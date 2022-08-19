using System;
using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.Blockchain.Web3;
using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Blockchain
{
    public class BlockchainService : IBlockchainService
    {
        private readonly IWeb3AccountService _web3AccountService;
        private readonly IContextProvider _contextProvider;
        private readonly ILogger _logger;

        public BlockchainService(IWeb3AccountService web3AccountService,
                                 IContextProvider contextProvider,
                                 ILoggerFactory loggerFactory)
        {
            _web3AccountService = web3AccountService;
            _contextProvider = contextProvider;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<string> CreateUser(string username)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var newUser = await _web3AccountService.CreateUser();
                    newUser.Username = username;
                    await context.BlockchainUsers.AddAsync(newUser);
                    await context.SaveChangesAsync();
                    return newUser.Address;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in CreateUser()", username);
                throw ex;
            }
        }

        public async Task<BlockchainInfo> GetBalances(string address)
        {
            try
            {
                var ethBalance = await _web3AccountService.EthBalance(address);
                return new BlockchainInfo
                {
                    Ether = ethBalance,
                    Address = address,
                    Coinstantine = 0
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetBalances()", address);
                throw ex;
            }
        }

        public async Task<string> DeployContract(SmartContract smartContract)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    await DeleteSmartContractIfExists(smartContract, x => x.IsMOCoinstantine, context);
                    await DeleteSmartContractIfExists(smartContract, x => x.IsCoinstantine, context);
                    await DeleteSmartContractIfExists(smartContract, x => x.IsSaleContract, context);
                    await DeleteSmartContractIfExists(smartContract, x => x.IsPresaleContract, context);

                    await context.SmartContracts.AddAsync(smartContract);
                    await context.SaveChangesAsync();
                    return smartContract.Address;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in DeployContract()", smartContract);
                throw ex;
            }
        }

        private async Task DeleteSmartContractIfExists(SmartContract smartContract, Func<SmartContract, bool> func, IContext context)
        {
            try
            {
                if (func(smartContract))
                {
                    var contracts = await context.SmartContracts.Include(s => s.Token)
                                                 .Where(x => func(x))
                                                 .ToListAsync();
                    foreach (var contract in contracts)
                    {
                        context.Tokens.Remove(contract.Token);
                        context.SmartContracts.Remove(contract);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteSmartContractIfExists()", smartContract);
                throw ex;
            }
        }
    }
}
