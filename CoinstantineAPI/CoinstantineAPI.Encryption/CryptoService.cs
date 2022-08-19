using CoinstantineAPI.Core.Encryption;
using Nethereum.KeyStore;

namespace CoinstantineAPI.Encryption
{
    public class CryptoService : ICryptoService
    {
        public string GenerateJson(string password, byte[] privateKey, string address)
        {
            var keyStore = new KeyStoreService();
            return keyStore.EncryptAndGenerateDefaultKeyStoreAsJson(password, privateKey, address);
        }
    }
}
