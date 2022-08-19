using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.Blockchain
{
    public interface IPrivateKeyRetreiver
    {
        Task<BlockchainUser> FromAccount(string accountAddress);
    }
}
