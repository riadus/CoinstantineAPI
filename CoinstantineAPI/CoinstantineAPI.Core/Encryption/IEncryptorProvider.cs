using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;

namespace CoinstantineAPI.Core.Encryption
{
    public interface IEncryptorProvider
    {
        IEncryptor GetEncryptor(EncryptorType encryptorType);
    }

    public interface ICryptoService
    {
        string GenerateJson(string password, byte[] privateKey, string address);
    }

    public interface IPasswordProvider
    {
        Task<string> GeneratePasswordFromBytes(byte[] bytes);
        Task<string> EncryptPasswordForUser(string password);
        Task<string> DecryptPasswordForUser(string password);
        Task<string> GetEncryptedOwnerPassword();
        Task<string> DecryptPasswordForOwner(string password);
    }

    public interface IPrivateKeyGenerator
    {
        byte[] GenerateNewPrivateKey();
        Task<byte[]> GetOwnerPrivateKey();
    }
}
