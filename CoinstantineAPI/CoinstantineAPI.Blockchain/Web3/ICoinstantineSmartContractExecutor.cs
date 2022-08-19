using System.Threading.Tasks;
using CoinstantineAPI.Data;
using Nethereum.RPC.Eth.DTOs;

namespace CoinstantineAPI.Blockchain.Web3
{
    public interface ICoinstantineSmartContractExecutor : ISmartContractExecutor
    {
        Task<T> GetOnSpecificToken<T>(Token token, string functionName, params object[] arguments);
        Task<TransactionReceipt> PostAndForgetOnSpecificToken(Token token, string functionName, Parameters param, params object[] arguments);
        Task<TransactionReceipt> PostAndForgetOnSpecificToken(Token token, string functionName, BlockchainUser sender, params object[] arguments);
    }
}