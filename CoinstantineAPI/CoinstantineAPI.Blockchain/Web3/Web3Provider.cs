using System;
using System.Threading.Tasks;
using CoinstantineAPI.Core;
using CoinstantineAPI.Core.Encryption;
using CoinstantineAPI.Data;
using Microsoft.Extensions.Logging;
using Nethereum.Geth;
using Nethereum.KeyStore;
using Nethereum.Web3.Accounts;

namespace CoinstantineAPI.Blockchain.Web3
{
    public class Web3Provider : IWeb3Provider
    {
        private readonly ILogger _logger;
        private readonly IEncryptorProvider _encryptorProvider;

        public Web3Provider(ILoggerFactory loggerFactory,
                            IEncryptorProvider encryptorProvider)
        {
            Web3 = new Web3Geth(Url);
            _logger = loggerFactory.CreateLogger(GetType());
            _encryptorProvider = encryptorProvider;
        }

        public Web3Geth Web3 { get; }

        public async Task<Web3Geth> GetWeb3ForUser(BlockchainUser blockchainUser)
        {
            try
            {
                var encryptorType = blockchainUser.Username == "Owner" ? EncryptorType.OwnerPassword : EncryptorType.UserPassword;

                var passphraseEncryptor = _encryptorProvider.GetEncryptor(encryptorType);
                var password = await passphraseEncryptor.Decrypt(blockchainUser.PassPhrase);

                var keyStoreService = new KeyStoreService();
                var privateKey = keyStoreService.DecryptKeyStoreFromJson(password, blockchainUser.Json);
                var account = new Account(privateKey);
                return new Web3Geth(account, Url);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetWeb3ForUser()", blockchainUser);
                throw ex;
            }
        }

        public string Url => Constants.Web3Url;
    }
}
