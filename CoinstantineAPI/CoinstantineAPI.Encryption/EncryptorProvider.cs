using System.Collections.Generic;
using CoinstantineAPI.Core;
using CoinstantineAPI.Core.Encryption;
using CoinstantineAPI.Encryption.Encryptors;

namespace CoinstantineAPI.Encryption
{
    public class EncryptorProvider : IEncryptorProvider
    {
        private readonly Dictionary<EncryptorType, IEncryptor> _encryptors;
        public EncryptorProvider(IKeyVaultCrypto keyVaultCrypto)
        {
            _encryptors = new Dictionary<EncryptorType, IEncryptor>
            {
                { EncryptorType.Passphrase, new Encryptor(keyVaultCrypto, Constants.PassphraseKey)},
                { EncryptorType.Phonenumber, new Encryptor(keyVaultCrypto, Constants.PhonenumberKey)},
                { EncryptorType.OwnerPrivateKey, new Encryptor(keyVaultCrypto, Constants.OwnerPrivateKey)},
                { EncryptorType.OwnerPassword, new Encryptor(keyVaultCrypto, Constants.OwnerPassword)},
                { EncryptorType.UserPassword, new Encryptor(keyVaultCrypto, Constants.UserPassword)},
                { EncryptorType.Jwt, new Encryptor(keyVaultCrypto, Constants.Jwt)},
            };
        }

        public IEncryptor GetEncryptor(EncryptorType encryptorType)
        {
            return _encryptors[encryptorType];
        }
    }
}
