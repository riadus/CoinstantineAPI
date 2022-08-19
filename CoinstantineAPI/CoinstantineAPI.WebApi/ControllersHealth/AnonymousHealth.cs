using System;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.WebApi.ControllersHealth
{
    [Route("healthz")]
    public class AnonymousHealth : BaseController
    {
        public AnonymousHealth(IUsersService userService, ILoggerFactory loggerFactory, IUserResolverService userResolverService) : base(userService, loggerFactory, userResolverService)
        {
        }

        [HttpGet]
        public ActionResult Healthz()
        {
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpGet("auth")]
        public ActionResult HealthzAuth()
        {
            return Ok();
        }
    }
}
