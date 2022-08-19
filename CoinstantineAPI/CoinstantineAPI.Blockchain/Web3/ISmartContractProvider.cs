using System.Threading.Tasks;
using CoinstantineAPI.Data;
using Nethereum.Contracts;

namespace CoinstantineAPI.Blockchain.Web3
{
    public interface ISmartContractProvider
    {
        Task<Contract> GetCoinstantine(BlockchainUser blockchainUser);
        Task<Contract> GetPresaleContract(BlockchainUser blockchainUser);
        Task<Contract> GetSmartContract(Token token);
    }
}
