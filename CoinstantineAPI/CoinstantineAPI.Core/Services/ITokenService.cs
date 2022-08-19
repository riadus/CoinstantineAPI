using System.Security.Claims;
using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.Services
{
    public interface ITokenService
    {
        string GenerateTokenFor(UserIdentity user);
        string GenerateRefreshToken();
        RefreshTokens GetRefreshTokenObject(string refreshToken);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }

    public interface IApplicationService
    {
        Task CreateApplication(string name, string description);
        Task CreateWebApplication(string clientId, string secret);
        Task CreateMobileApplication(string clientId, string secret);
        Application GenerateIds();
    }
}
