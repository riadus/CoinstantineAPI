using System.Threading.Tasks;
using CoinstantineAPI.Core.Extensions;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.Services
{
    public interface IAuthService
    {
        Task<UserIdentity> Authenticate(string email, string password);
        Task UpdateRefreshToken(UserIdentity user, RefreshTokens refreshToken, ApplicationClient applicationClient);
        Task CreateRefreshToken(UserIdentity user, RefreshTokens refreshToken, ApplicationClient applicationClient);
        bool VerifyRefreshToken(UserIdentity user, string refreshToken, ApplicationClient applicationClient);
    }

    public class ApplicationClient
    {
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public bool IsValid => ClientId.IsNotNull() && Secret.IsNotNull();
    }
}
