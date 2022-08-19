using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.WebKey;

namespace CoinstantineAPI.Encryption
{
    public class KeyVaultCrypto : IKeyVaultCrypto
    {
        private readonly KeyVaultClient client;

        public KeyVaultCrypto(KeyVaultClient client)
        {
            this.client = client;
        }

        public async Task<string> DecryptAsync(string keyId, string encryptedText)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            var decryptionResult = await client.DecryptAsync(keyId,
                                     JsonWebKeyEncryptionAlgorithm.RSAOAEP, encryptedBytes);
            var decryptedText = Encoding.Unicode.GetString(decryptionResult.Result);
            return decryptedText;
        }

        public async Task<string> EncryptAsync(string keyId, string value)
        {
            var bundle = await client.GetKeyAsync(keyId);
            var key = bundle.Key;

            using (var rsa = new RSACryptoServiceProvider())
            {
                var parameters = new RSAParameters()
                {
                    Modulus = key.N,
                    Exponent = key.E
                };
                rsa.ImportParameters(parameters);
                var byteData = Encoding.Unicode.GetBytes(value);
                var encryptedText = rsa.Encrypt(byteData, fOAEP: true);
                var encodedText = Convert.ToBase64String(encryptedText);
                return encodedText;
            }
        }

        public async Task<string> GetSecret(string keyId)
        {
            var bundle = await client.GetSecretAsync(keyId);
            return bundle.Value;
        }
    }
}
