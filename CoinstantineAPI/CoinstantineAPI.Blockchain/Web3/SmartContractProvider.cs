using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;
using Nethereum.Contracts;
using Nethereum.Geth;

namespace CoinstantineAPI.Blockchain.Web3
{
    public class SmartContractProvider : ISmartContractProvider
    {
        private readonly IContextProvider _contextProvider;
        private readonly IWeb3Provider _web3Provider;
        private readonly Web3Geth _web3;
        public SmartContractProvider(IContextProvider contextProvider, 
                                     IWeb3Provider web3Provider)
        {
            _contextProvider = contextProvider;
            _web3Provider = web3Provider;
            _web3 = web3Provider.Web3;
        }

        public async Task<Contract> GetCoinstantine(BlockchainUser blockchainUser)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                var smartContract = await context.SmartContracts.Include(s => s.Token)
                                                 .FirstAsync(x => x.IsMOCoinstantine);
                var abi = smartContract.Abi;
                Web3Geth web3 = null;
                if (blockchainUser == null)
                {
                    web3 = _web3;
                }
                else
                {
                    web3 = await _web3Provider.GetWeb3ForUser(blockchainUser);
                }
                return web3.Eth.GetContract(abi, smartContract.Address);
            }
        }

        public async Task<Contract> GetPresaleContract(BlockchainUser blockchainUser)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                var smartContract = await context.SmartContracts.Include(s => s.Token)
                                                     .FirstAsync(x => x.IsPresaleContract); 
                var abi = smartContract.Abi;
                Web3Geth web3 = null;
                if (blockchainUser == null)
                {
                    web3 = _web3;
                }
                else
                {
                    web3 = await _web3Provider.GetWeb3ForUser(blockchainUser);
                }
                return web3.Eth.GetContract(abi, smartContract.Address);
            }
        }

        public async Task<Contract> GetSmartContract(Token token)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                var smartContract = await context.SmartContracts.Include(s => s.Token)
                                                     .FirstAsync(x => x.IsCoinstantine);
                var abi = smartContract.Abi;
                return _web3.Eth.GetContract(abi, smartContract.Address);
            }
        }
    }
}
