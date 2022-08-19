using System;
using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Data;
using Microsoft.Extensions.Logging;
using Nethereum.Contracts;
using Nethereum.Geth;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace CoinstantineAPI.Blockchain.Web3
{
    public class SmartContractExecutor : ISmartContractExecutor
    {
        protected readonly Web3Geth _web3;
        protected readonly ISmartContractProvider _smartContractProvider;
        protected readonly IWeb3AccountService _web3AccountService;
        private readonly ISmartContractFactory _smartContractFactory;
        protected SmartContractType _smartContractType;
        protected readonly ILogger _logger;

        public SmartContractExecutor(IWeb3Provider web3Provider,
                                     IWeb3AccountService web3AccountService,
                                     ISmartContractFactory smartContractFactory,
                                     ILoggerFactory loggerFactory)
        {
            _web3 = web3Provider.Web3;
            _web3AccountService = web3AccountService;
            _smartContractFactory = smartContractFactory;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public void SetSmartContractType(SmartContractType smartContractType)
        {
            _smartContractType = smartContractType;
        }

        protected Task<Contract> GetSmartContract()
        {
            try
            {
                return _smartContractFactory.GetContractFor(_smartContractType);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetSmartContract()");
                throw ex;
            }
        }

        protected Task<Contract> GetSmartContract(BlockchainUser blockchainUser)
        {
            try
            {
                return _smartContractFactory.GetContractFor(_smartContractType, blockchainUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSmartContract()", blockchainUser);
                throw ex;
            }
        }

        public async Task<T> Get<T>(string functionName, params object[] arguments)
        {
            try
            {
                var smartContract = await GetSmartContract().ConfigureAwait(false);
                return await GetAsync<T>(smartContract, functionName, arguments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get<T>()", typeof(T), functionName, arguments);
                throw ex;
            }
        }

        protected async Task<T> GetAsync<T>(Contract contract, string functionName, params object[] arguments)
        {
            try
            {
                var function = contract.GetFunction(functionName);
                var result = await function.CallAsync<T>(arguments);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAsync<T>()", typeof(T), functionName, arguments);
                throw ex;
            }
        }

        public async Task<T> GetForTuple<T>(string functionName, params object[] arguments) where T : new()
        {
            try
            {
                var smartContract = await GetSmartContract().ConfigureAwait(false);
                var function = smartContract.GetFunction(functionName);
                var result = await function.CallDeserializingToObjectAsync<T>(arguments);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetForTuple<T>()", typeof(T), functionName, arguments);
                throw ex;
            }
        }

        private string GetEventName(string functionName)
        {
            try
            {
                var firstChar = functionName[0].ToString().ToUpper();
                var restOfString = functionName.Substring(1);
                return firstChar + restOfString;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetEventName()", functionName);
                throw ex;
            }
        }

        public async Task<(T, TransactionReceipt)> PostWithResult<T>(string functionName, Parameters param, params object[] arguments) where T : new()
        {
            try
            {
                var contract = await GetSmartContract(param.Sender).ConfigureAwait(false);

                var eventFilter = contract.GetEvent(GetEventName(functionName));

                var transactionReceipt = await ExecuteFunctionOnContract(contract, functionName, param, arguments);

                var filterInput = eventFilter.CreateFilterInput(new BlockParameter(transactionReceipt.BlockNumber), BlockParameter.CreateLatest());

                var result = await eventFilter.GetAllChanges<T>(filterInput).ConfigureAwait(false);
                if (result?.Any() ?? false)
                {
                    foreach (var eventResult in result)
                    {
                        if (eventResult.Log.TransactionHash == transactionReceipt.TransactionHash)
                        {
                            var logs = eventResult.Log;
                            return (eventResult.Event, transactionReceipt);
                        }
                    }
                    return (result[0].Event, transactionReceipt);
                }
                throw new Exception("Error on smart contract");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in PostWithResult<T>()", typeof(T), param, arguments);
                throw ex;
            }
        }

        public async Task<(T, TransactionReceipt)> GetResultOfPreviousFunction<T>(TransactionReceipt transactionReceipt, string functionName, Parameters param) where T : new()
        {
            try
            {
                var contract = await GetSmartContract(param.Sender).ConfigureAwait(false);

                var eventFilter = contract.GetEvent(GetEventName(functionName));

                var filterInput = eventFilter.CreateFilterInput(new BlockParameter(transactionReceipt.BlockNumber), BlockParameter.CreateLatest());

                var result = await eventFilter.GetAllChanges<T>(filterInput).ConfigureAwait(false);
                if (result?.Any() ?? false)
                {
                    foreach (var eventResult in result)
                    {
                        if (eventResult.Log.TransactionHash == transactionReceipt.TransactionHash)
                        {
                            var logs = eventResult.Log;
                            return (eventResult.Event, transactionReceipt);
                        }
                    }
                    return (result[0].Event, transactionReceipt);
                }
                throw new Exception("Error on smart contract");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetResultOfPreviousFunction<T>()", typeof(T), functionName, transactionReceipt);
                throw ex;
            }
        }

        public async Task<TransactionReceipt> PostAsync(string functionName, Parameters param, params object[] arguments)
        {
            try
            {
                var contract = await GetSmartContract(param.Sender).ConfigureAwait(false);
                return await ExecuteFunctionOnContract(contract, functionName, param, arguments);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in PostAsync()", functionName, param, arguments);
                throw ex;
            }
        }

        public async Task<(T, TransactionReceipt)> PostWithResult<T>(string eventName, BlockchainUser sender, params object[] arguments) where T : new()
        {
            try
            {
                return await PostWithResult<T>(eventName, new Parameters { Sender = sender }, arguments).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in PostWithResult<T>()", typeof(T), sender, arguments);
                throw ex;
            }
        }

        public async Task<TransactionReceipt> PostAndForget(string functionName, Parameters param, params object[] arguments)
        {
            try
            {
                var smartContract = await GetSmartContract(param.Sender).ConfigureAwait(false);
                return await ExecuteFunctionOnContract(smartContract, functionName, param, arguments).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in PostAndForget()", functionName, param, arguments);
                throw ex;
            }
        }

        public async Task<TransactionReceipt> PostAndForget(string functionName, BlockchainUser sender, params object[] arguments)
        {
            try
            {
                var smartContract = await GetSmartContract(sender).ConfigureAwait(false);
                return await ExecuteFunctionOnContract(smartContract, functionName, new Parameters { Sender = sender }, arguments).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in PostAndForget()", functionName, sender, arguments);
                throw ex;
            }
        }

        public async Task<string> PostAndDontWaitForTransactionReceipt(string functionName, Parameters param, params object[] arguments)
        {
            try
            {
                var smartContract = await GetSmartContract(param.Sender).ConfigureAwait(false);
                return await ExecuteFunctionOnContractAndDontWait(smartContract, functionName, param, arguments).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PostAndDontWaitForTransactionReceipt()", functionName, param, arguments);
                throw ex;
            }
        }

        protected async Task<TransactionReceipt> ExecuteFunctionOnContract(Contract contract, string functionName, Parameters param, params object[] arguments)
        {
            try
            {
                var function = contract.GetFunction(functionName);
                var blockNumber = BlockParameter.CreateLatest();
                var block = await _web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(blockNumber);
                var gasLimit = block.GasLimit.Value;
                var estimationGas = new HexBigInteger(gasLimit);
                var valueHex = param.Value == null ? null : new HexBigInteger(param.Value.Value);
                HexBigInteger estimatedGas;
                try
                {
                    estimatedGas = await function.EstimateGasAsync(param.Sender.Address, estimationGas, valueHex,
                                                                              arguments).ConfigureAwait(false);
                }
                catch(Exception gasException)
                {
                    estimatedGas = new HexBigInteger(gasLimit);
                    _logger.LogError(gasException, "Error in ExecuteFunctionOnContract() for EstimateGasAsync()", contract, functionName, param, arguments);
                }
                return await function.SendTransactionAndWaitForReceiptAsync(param.Sender.Address, estimatedGas, valueHex, null,
                                                                        arguments).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in ExecuteFunctionOnContract()", contract, functionName, param, arguments);
                throw ex;
            }
        }

        protected async Task<string> ExecuteFunctionOnContractAndDontWait(Contract contract, string functionName, Parameters param, params object[] arguments)
        {
            try
            {
                var function = contract.GetFunction(functionName);
                var blockNumber = BlockParameter.CreateLatest();
                var block = await _web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(blockNumber);
                var gasLimit = block.GasLimit.Value;
                var estimationGas = new HexBigInteger(gasLimit);
                var valueHex = param.Value == null ? null : new HexBigInteger(param.Value.Value);
                HexBigInteger estimatedGas;
                try
                {
                    estimatedGas = await function.EstimateGasAsync(param.Sender.Address, estimationGas, valueHex,
                                                                              arguments).ConfigureAwait(false);
                }
                catch (Exception gasException)
                {
                    estimatedGas = new HexBigInteger(gasLimit);
                    _logger.LogError(gasException, "Error in ExecuteFunctionOnContractAndDontWait() for EstimateGasAsync()", contract, functionName, param, arguments);
                }
                return await function.SendTransactionAsync(param.Sender.Address, estimatedGas, valueHex, arguments).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ExecuteFunctionOnContractAndDontWait()", contract, functionName, param, arguments);
                throw ex;
            }
        }

        public async Task<TransactionReceipt> GetReceiptFromHash(string transactionHash)
        {
            var rec = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            return rec;
        }
    }
}
