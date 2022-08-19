using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoinstantineAPI.WebApi.Controllers
{
    public interface IUserResolverService
    {
        ClaimsPrincipal User { get; }
        string ClientId { get; }
        string Secret { get; }
    }
}
