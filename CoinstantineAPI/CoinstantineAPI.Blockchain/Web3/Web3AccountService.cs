using System;
using System.Threading;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Encryption;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nethereum.Geth;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.KeyStore;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3.Accounts;

namespace CoinstantineAPI.Blockchain.Web3
{
    public class Web3AccountService : IWeb3AccountService
    {
        private readonly Web3Geth _web3;
        private readonly IContextProvider _contextProvider;
        private readonly ICryptoService _cryptoService;
        private readonly IEncryptorProvider _encryptorProvider;
        private readonly IPasswordProvider _passwordProvider;
        private readonly IPrivateKeyGenerator _privateKeyGenerator;
        private readonly ILogger _logger;

        public Web3AccountService(IWeb3Provider web3Provider,
                                  IContextProvider contextProvider,
                                  ICryptoService cryptoService,
                                  IEncryptorProvider encryptorProvider,
                                  IPasswordProvider passwordProvider,
                                  IPrivateKeyGenerator privateKeyGenerator,
                                  ILoggerFactory loggerFactory)
        {
            _web3 = web3Provider.Web3;
            _contextProvider = contextProvider;
            _cryptoService = cryptoService;
            _encryptorProvider = encryptorProvider;
            _passwordProvider = passwordProvider;
            _privateKeyGenerator = privateKeyGenerator;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<BlockchainUser> GetOwnerAndCreateIfNeeded()
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var owner = await context.BlockchainUsers.FirstOrDefaultAsync(x => x.Username == "Owner");
                    if (owner == null)
                    {
                        var privateKey = await _privateKeyGenerator.GetOwnerPrivateKey();
                        var account = new Account(privateKey);

                        var encryptedPassphrase = await _passwordProvider.GetEncryptedOwnerPassword();
                        var passphrase = await _passwordProvider.DecryptPasswordForOwner(encryptedPassphrase);

                        var json = _cryptoService.GenerateJson(passphrase, privateKey, account.Address);

                        owner = new BlockchainUser { Address = account.Address, PassPhrase = encryptedPassphrase, Json = json, Username = "Owner" };
                        await context.BlockchainUsers.AddAsync(owner);
                        await context.SaveChangesAsync();
                    }
                    return owner;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetOwnerAndCreateIfNeeded()");
                throw ex;
            }
        }

        public async Task<BlockchainUser> CreateUser()
        {
            try
            {
                var privateKey = _privateKeyGenerator.GenerateNewPrivateKey();

                var password = await _passwordProvider.GeneratePasswordFromBytes(privateKey);
                var encryptedPassword = await _passwordProvider.EncryptPasswordForUser(password);

                var account = new Account(privateKey);

                var json = _cryptoService.GenerateJson(password, privateKey, account.Address);

                return new BlockchainUser { Address = account.Address, PassPhrase = encryptedPassword, Json = json };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateUser()");
                throw ex;
            }
        }

        private async Task<TransactionReceipt> MineAndGetReceiptAsync(string transactionHash)
        {
            try
            {
                var miningResult = await _web3.Miner.Start.SendRequestAsync(6);

                var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

                while (receipt == null)
                {
                    Thread.Sleep(1000);
                    receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
                }

                miningResult = await _web3.Miner.Stop.SendRequestAsync();
                return receipt;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in MineAndGetReceiptAsync()", transactionHash);
                throw ex;
            }
        }

        public async Task<float> EthBalance(string address)
        {
            try
            {
                var balance = await _web3.Eth.GetBalance.SendRequestAsync(address);
                return (float)Nethereum.Web3.Web3.Convert.FromWei(balance, UnitConversion.EthUnit.Ether);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in EthBalance()", address);
                throw ex;
            }
        }
    }
}
