using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.Blockchain
{
    public interface IBlockchainService
    {
        Task<string> CreateUser(string username);
        Task<BlockchainInfo> GetBalances(string address);
        Task<string> DeployContract(SmartContract smartContract);
    }
}
