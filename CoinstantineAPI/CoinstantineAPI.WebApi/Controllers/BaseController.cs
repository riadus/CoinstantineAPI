using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoinstantineAPI.WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IUsersService _userService;
        private readonly IUserResolverService _userResolverService;
        protected readonly ILogger _logger;

        protected BaseController(IUsersService userService, ILoggerFactory loggerFactory, IUserResolverService userResolverService)
        {
            _userService = userService;
            _userResolverService = userResolverService;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected ApplicationClient GetApplicationClient()
        {
            return new ApplicationClient
            {
                ClientId = _userResolverService.ClientId,
                Secret = _userResolverService.ClientId
            };
        }

        protected AzureUser GetUser()
        {
            var user = _userResolverService.User;
            var userId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var role = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

            return new AzureUser
            {
                EmailAddress = email,
                UserId = userId,
                Role = Enum.Parse<UserRole>(role)
            };
        }

        protected async Task<ApiUser> GetApiUser()
        {
            var user = GetUser();
            return await _userService.GetUserFromEmail(user.EmailAddress);
        }

        protected async Task<ApiUser> GetApiUserFromEmail(string email)
        {
            return await _userService.GetUserFromEmail(email);
        }

        protected async Task<BlockchainUser> GetBlockchainUser()
        {
            var user = GetUser();
            return await _userService.GetBlockchainUserFromEmail(user.EmailAddress);
        }
    }
}
