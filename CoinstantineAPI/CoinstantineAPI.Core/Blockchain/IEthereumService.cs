using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.Blockchain
{
    public interface IEthereumService
    {
        Task<string> SendFunds(BlockchainUser from, string to);
        Task<decimal> GetMaximumUsableEtherFor(BlockchainUser user, decimal gas);
        Task<decimal> GetGasPrice();
    }
}
