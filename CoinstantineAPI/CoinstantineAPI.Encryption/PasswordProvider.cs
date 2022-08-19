using System.Threading.Tasks;
using CoinstantineAPI.Core.Encryption;
using Nethereum.Hex.HexConvertors.Extensions;

namespace CoinstantineAPI.Encryption
{
    public class PasswordProvider : IPasswordProvider
    {
        private readonly IEncryptorProvider _encryptorProvider;
        public PasswordProvider(IEncryptorProvider encryptorProvider)
        {
            _encryptorProvider = encryptorProvider;
        }

        public virtual async Task<string> GeneratePasswordFromBytes(byte[] bytes)
        {
            var passphraseEncryptor = _encryptorProvider.GetEncryptor(EncryptorType.UserPassword);
            var encryptedPassphrase = await passphraseEncryptor.GetSecret();
            var passphrase = await passphraseEncryptor.Decrypt(encryptedPassphrase);

            return bytes.ToHex() + passphrase;
        }

        public async Task<string> EncryptPasswordForUser(string password)
        {
            return await EncryptPasswordFor(password, EncryptorType.UserPassword);
        }

        public async Task<string> DecryptPasswordForUser(string password)
        {
            return await DecryptPasswordFor(password, EncryptorType.UserPassword);
        }

        private async Task<string> EncryptPasswordFor(string password, EncryptorType encryptorType)
        {
            var passphraseEncryptor = _encryptorProvider.GetEncryptor(encryptorType);
            return await passphraseEncryptor.Encrypt(password);
        }

        private async Task<string> DecryptPasswordFor(string password, EncryptorType encryptorType)
        {
            var passphraseEncryptor = _encryptorProvider.GetEncryptor(encryptorType);
            return await passphraseEncryptor.Decrypt(password);
        }

        public async Task<string> GetEncryptedOwnerPassword()
        {
            var passphraseEncryptor = _encryptorProvider.GetEncryptor(EncryptorType.OwnerPassword);
            return await passphraseEncryptor.GetSecret();
        }

        public async Task<string> DecryptPasswordForOwner(string password)
        {
            return await DecryptPasswordFor(password, EncryptorType.OwnerPassword);
        }
    }
}
