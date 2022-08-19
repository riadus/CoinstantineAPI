using System.Threading.Tasks;
using CoinstantineAPI.Data;
using Nethereum.Geth;

namespace CoinstantineAPI.Blockchain.Web3
{
    public interface IWeb3Provider
    {
        Web3Geth Web3 { get; }
        Task<Web3Geth> GetWeb3ForUser(BlockchainUser blockchainUser);
        string Url { get; }
    }
}
