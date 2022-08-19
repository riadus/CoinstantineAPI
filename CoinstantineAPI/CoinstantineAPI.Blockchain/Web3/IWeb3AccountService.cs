using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Blockchain.Web3
{
    public interface IWeb3AccountService
    {
        Task<BlockchainUser> CreateUser();
        Task<BlockchainUser> GetOwnerAndCreateIfNeeded();
        Task<float> EthBalance(string address);
    }
}