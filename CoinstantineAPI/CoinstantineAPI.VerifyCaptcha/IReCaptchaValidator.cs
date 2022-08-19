using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CoinstantineAPI.VerifyCaptcha
{
    public interface IReCaptchaValidator
    {
        Task<VerifyReCaptchaResponse> Validate(IHeaderDictionary header);
    }
}
