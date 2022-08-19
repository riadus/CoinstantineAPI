using System.Threading.Tasks;

namespace CoinstantineAPI.VerifyCaptcha
{
    public interface ICheckCaptcha
    {
        Task<bool> ShouldCheck(string clientId, string secret);
    }
}
