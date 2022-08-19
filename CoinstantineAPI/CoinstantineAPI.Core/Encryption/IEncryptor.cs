using System.Threading.Tasks;

namespace CoinstantineAPI.Core.Encryption
{
    public interface IEncryptor
    {
        Task<string> Encrypt(string value);
        Task<string> Decrypt(string cryptedValue);
        Task<string> GetSecret();
    }
}
