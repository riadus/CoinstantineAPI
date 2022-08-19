using System.Threading.Tasks;
using CoinstantineAPI.Core;
using CoinstantineAPI.Core.Encryption;

namespace CoinstantineAPI.Encryption.Encryptors
{
    public class Encryptor : IEncryptor
    {
        private readonly IKeyVaultCrypto _keyVaultCrypto;
        private readonly string _key;
        private readonly string _secret;

        public Encryptor(IKeyVaultCrypto keyVaultCrypto, string keyId)
        {
            _keyVaultCrypto = keyVaultCrypto;
            var url = Constants.VaultUrl;
            _key = $"{url}/keys/{keyId}";
            _secret = $"{url}/secrets/{keyId}";
        }

        public Task<string> Decrypt(string cryptedValue)
        {
            return _keyVaultCrypto.DecryptAsync(_key, cryptedValue);
        }

        public Task<string> Encrypt(string value)
        {
            return _keyVaultCrypto.EncryptAsync(_key, value);
        }

        public Task<string> GetSecret()
        {
            return _keyVaultCrypto.GetSecret(_secret);
        }
    }
}
