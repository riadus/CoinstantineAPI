using System.Threading.Tasks;
using CoinstantineAPI.Data;
using Nethereum.Contracts;

namespace CoinstantineAPI.Blockchain.Web3
{
    public interface ISmartContractFactory
    {
        Task<Contract> GetContractFor(SmartContractType type);
        Task<Contract> GetContractFor(SmartContractType type, BlockchainUser blockchainUser);
    }
}
