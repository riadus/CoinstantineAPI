using System.Threading.Tasks;
using CoinstantineAPI.Core.Encryption;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;

namespace CoinstantineAPI.Encryption
{
    public class PrivateKeyGenerator : IPrivateKeyGenerator
    {
        private readonly IEncryptorProvider _encryptorProvider;

        public PrivateKeyGenerator(IEncryptorProvider encryptorProvider)
        {
            _encryptorProvider = encryptorProvider;
        }

        public byte[] GenerateNewPrivateKey()
        {
            var ecKey = EthECKey.GenerateKey();
            return ecKey.GetPrivateKeyAsBytes();
        }

        public async Task<byte[]> GetOwnerPrivateKey()
        {
            var privateKeyEncryptor = _encryptorProvider.GetEncryptor(EncryptorType.OwnerPrivateKey);
            var encryptedPrivateKey = await privateKeyEncryptor.GetSecret();
            var privateKey = await privateKeyEncryptor.Decrypt(encryptedPrivateKey);
            return privateKey.HexToByteArray();
        }
    }
}
