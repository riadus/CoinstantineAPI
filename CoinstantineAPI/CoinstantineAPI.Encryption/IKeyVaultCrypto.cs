using System.Threading.Tasks;

namespace CoinstantineAPI.Encryption
{
    public interface IKeyVaultCrypto
    {
        Task<string> EncryptAsync(string keyId, string value);
        Task<string> DecryptAsync(string keyId, string encryptedText);
        Task<string> GetSecret(string keyId);
    }
}
