using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Core.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static CoinstantineAPI.WebApi.Controllers.RefreshTokenController;

namespace CoinstantineAPI.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [Route("api/[controller]")]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IAuthService authService,
                                        IUsersService userService,
                                        ILoggerFactory loggerFactory,
                                        IUserResolverService userResolverService,
                                        ITokenService tokenService) : base(userService, loggerFactory, userResolverService)
        {
            this._authService = authService;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> Authenticate()
        {
            var applicationClient = GetApplicationClient();
            var email = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email);
            var user = await _userService.GetUserIdentityFromEmail(email.Value);
            if (!applicationClient.IsValid)
            {
                return new StatusCodeResult(StatusCodes.Status412PreconditionFailed);
            }

            if (!user?.EmailConfirmed ?? true)
            {
                return StatusCode(StatusCodes.Status423Locked, new { message = "Email not confirmed" });
            }

            var tokenString = _tokenService.GenerateTokenFor(user);
            var refreshTokenString = _tokenService.GenerateRefreshToken();
            var refreshToken = _tokenService.GetRefreshTokenObject(refreshTokenString);
            await _authService.CreateRefreshToken(user, refreshToken, applicationClient);
            var expirationDate = new DateTimeOffset(refreshToken.ExpirationDate).ToUnixTimeSeconds();

            return Ok(new Tokens
            {
                RefreshToken = refreshTokenString,
                Token = tokenString,
                ExpirationDate = expirationDate
            });
        }
    }
}
