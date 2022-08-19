using System;
using System.Threading.Tasks;
using CoinstantineAPI.Data;
using Nethereum.Contracts;

namespace CoinstantineAPI.Blockchain.Web3
{
    public class SmartContractFactory : ISmartContractFactory
    {
        private readonly ISmartContractProvider _smartContractProvider;

        public SmartContractFactory(ISmartContractProvider smartContractProvider)
        {
            _smartContractProvider = smartContractProvider;
        }

        public Task<Contract> GetContractFor(SmartContractType type, BlockchainUser blockchainUser)
        {
            switch (type)
            {
                case SmartContractType.Coinstantine:
                    return _smartContractProvider.GetCoinstantine(blockchainUser);
                case SmartContractType.Presale:
                    return _smartContractProvider.GetPresaleContract(blockchainUser);
            }
            throw new InvalidOperationException("SmartContractType needs to be defined");
        }

        public Task<Contract> GetContractFor(SmartContractType type)
        {
            return GetContractFor(type, null);
        }
    }
}
