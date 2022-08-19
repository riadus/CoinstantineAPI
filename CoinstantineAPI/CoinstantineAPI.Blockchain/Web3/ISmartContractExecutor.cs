using System.Threading.Tasks;
using CoinstantineAPI.Data;
using Nethereum.RPC.Eth.DTOs;

namespace CoinstantineAPI.Blockchain.Web3
{
    public interface ISmartContractExecutor
    {
        void SetSmartContractType(SmartContractType smartContractType);

        Task<T> Get<T>(string functionName, params object[] arguments);
        Task<T> GetForTuple<T>(string functionName, params object[] arguments) where T : new();

        Task<(T Event, TransactionReceipt Receipt)> PostWithResult<T>(string functionName, Parameters param, params object[] arguments) where T : new();
        Task<(T Event, TransactionReceipt Receipt)> PostWithResult<T>(string functionName, BlockchainUser sender, params object[] arguments) where T : new();

        Task<TransactionReceipt> PostAndForget(string functionName, Parameters param, params object[] arguments);
        Task<TransactionReceipt> PostAndForget(string functionName, BlockchainUser sender, params object[] arguments);

        Task<string> PostAndDontWaitForTransactionReceipt(string functionName, Parameters param, params object[] arguments);
        Task<(T, TransactionReceipt)> GetResultOfPreviousFunction<T>(TransactionReceipt transactionReceipt, string functionName, Parameters param) where T : new();
        Task<TransactionReceipt> GetReceiptFromHash(string transactionHash);
    }
}