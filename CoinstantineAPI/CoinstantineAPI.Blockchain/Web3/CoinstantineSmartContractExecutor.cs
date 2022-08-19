using System;
using System.Threading.Tasks;
using CoinstantineAPI.Data;
using Microsoft.Extensions.Logging;
using Nethereum.RPC.Eth.DTOs;

namespace CoinstantineAPI.Blockchain.Web3
{
    public class CoinstantineSmartContractExecutor : SmartContractExecutor, ICoinstantineSmartContractExecutor
    {
        public CoinstantineSmartContractExecutor(IWeb3Provider web3Provider,
                                                 ISmartContractFactory smartContractFactory,
                                                 IWeb3AccountService web3AccountService,
                                                 ILoggerFactory loggerFactory) : base(web3Provider, web3AccountService, smartContractFactory, loggerFactory)
        {
            SetSmartContractType(SmartContractType.Coinstantine);
        }

        public async Task<TransactionReceipt> PostAndForgetOnSpecificToken(Token token, string functionName, Parameters param, params object[] arguments)
        {
            try
            {
                var contract = await _smartContractProvider.GetSmartContract(token).ConfigureAwait(false);
                return await ExecuteFunctionOnContract(contract, functionName, param, arguments);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in PostAndForgetOnSpecificToken()", token, functionName, param, arguments);
                throw ex;
            }
        }

        public async Task<TransactionReceipt> PostAndForgetOnSpecificToken(Token token, string functionName, BlockchainUser sender, params object[] arguments)
        {
            try
            {
                var contract = await _smartContractProvider.GetSmartContract(token).ConfigureAwait(false);
                return await ExecuteFunctionOnContract(contract, functionName, new Parameters { Sender = sender }, arguments).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in PostAndForgetOnSpecificToken()", token, functionName, sender, arguments);
                throw ex;
            }
        }

        public async Task<T> GetOnSpecificToken<T>(Token token, string functionName, params object[] arguments)
        {
            try
            {
                var contract = await _smartContractProvider.GetSmartContract(token).ConfigureAwait(false);
                return await GetAsync<T>(contract, functionName, arguments);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetOnSpecificToken<T>()", typeof(T), functionName, arguments);
                throw ex;
            }
        }
    }
}
