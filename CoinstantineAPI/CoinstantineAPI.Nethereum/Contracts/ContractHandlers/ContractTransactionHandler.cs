using System.Threading;
using System.Threading.Tasks;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Accounts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.TransactionManagers;

namespace Nethereum.Contracts.ContractHandlers
{
#if !DOTNET35
    public class ContractTransactionHandler<TContractMessage> : ContractTransactionHandlerBase, 
        IContractTransactionHandler<TContractMessage> where TContractMessage : FunctionMessage, new()
    {
        private ITransactionEstimatorHandler<TContractMessage> _estimatorHandler;
        private ITransactionReceiptPollHandler<TContractMessage> _receiptPollHandler;
        private ITransactionSenderHandler<TContractMessage> _transactionSenderHandler;
        private ITransactionSigner<TContractMessage> _transactionSigner;


        public ContractTransactionHandler(ITransactionManager transactionManager) : base(transactionManager)
        {
            _estimatorHandler = new TransactionEstimatorHandler<TContractMessage>(transactionManager);
            _receiptPollHandler = new TransactionReceiptPollHandler<TContractMessage>(transactionManager);
            _transactionSenderHandler = new TransactionSenderHandler<TContractMessage>(transactionManager);
            _transactionSigner = new TransactionSigner<TContractMessage>(transactionManager);
        }

        public Task<string> SignTransactionAsync(
            string contractAddress, TContractMessage functionMessage = null)
        {
            return _transactionSigner.SignTransactionAsync(contractAddress, functionMessage);
        }

        public Task<TransactionReceipt> SendRequestAndWaitForReceiptAsync(
            string contractAddress, TContractMessage functionMessage = null, CancellationTokenSource tokenSource = null)
        {
           return _receiptPollHandler.SendTransactionAsync(contractAddress, functionMessage, tokenSource);
        }

        public Task<string> SendRequestAsync(string contractAddress, TContractMessage functionMessage = null)
        {
            return _transactionSenderHandler.SendTransactionAsync(contractAddress, functionMessage);
        }

        public async Task<TransactionInput> CreateTransactionInputEstimatingGasAsync(
            string contractAddress, TContractMessage functionMessage = null)
        {
            var gasEstimate = await EstimateGasAsync(contractAddress, functionMessage).ConfigureAwait(false);
            functionMessage.Gas = gasEstimate;
            return functionMessage.CreateTransactionInput(contractAddress);
        }

        public Task<HexBigInteger> EstimateGasAsync(string contractAddress, TContractMessage functionMessage = null)
        {
            return _estimatorHandler.EstimateGasAsync(contractAddress, functionMessage);
        }
    }
#endif

}