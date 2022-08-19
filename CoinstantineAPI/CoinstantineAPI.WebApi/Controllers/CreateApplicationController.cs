using System.Threading.Tasks;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.WebApi.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.WebApi.Controllers
{
    [AuthorizeAdminOnly(AuthenticationSchemes = "BasicAuthentication")]
    [Route("api/[controller]")]
    public class CreateApplicationController : BaseController
    {
        private readonly IApplicationService _applicationService;

        public CreateApplicationController(IUsersService userService,
                                           ILoggerFactory loggerFactory,
                                           IApplicationService applicationService,
                                           IUserResolverService userResolverService) : base(userService, loggerFactory, userResolverService)
        {
            _applicationService = applicationService;
        }

        [HttpGet("new")]
        public IActionResult GenerateIdentifiers()
        {
            return Ok(_applicationService.GenerateIds());
        }

        [HttpPost("web")]
        public async Task<IActionResult> CreateWebApplication([FromQuery]string clientId, [FromQuery]string secret)
        {
            await _applicationService.CreateWebApplication(clientId, secret);
            return Ok();
        }

        [HttpPost("mobile")]
        public async Task<IActionResult> CreateMobileApplication([FromQuery]string clientId, [FromQuery]string secret)
        {
            await _applicationService.CreateMobileApplication(clientId, secret);
            return Ok();
        }
    }
}
