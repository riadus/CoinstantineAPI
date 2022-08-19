using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoinstantineAPI.WebApi.Controllers
{
    public class UserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor _context;
        public UserResolverService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public ClaimsPrincipal User => _context.HttpContext?.User;

        public string ClientId => GetValueFromHeader("client_id");
        public string Secret => GetValueFromHeader("secret");

        private string GetValueFromHeader(string key)
        {
            return _context.HttpContext?.Request?.Headers?.ContainsKey(key) ?? false ? _context.HttpContext.Request.Headers[key].FirstOrDefault() : null;
        }
    }
}
