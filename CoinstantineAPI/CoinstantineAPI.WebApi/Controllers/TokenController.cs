using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Core.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class RefreshTokenController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public RefreshTokenController(IAuthService authService,
                                      IUsersService userService,
                                      ILoggerFactory loggerFactory,
                                      IUserResolverService userResolverService,
                                      ITokenService tokenService) : base(userService, loggerFactory, userResolverService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody]Tokens tokens)
        {
            var applicationClient = GetApplicationClient();
            if(!applicationClient.IsValid)
            {
                return new StatusCodeResult(StatusCodes.Status412PreconditionFailed);
            }
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokens.Token);
            var email = principal.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            var user = await _userService.GetUserIdentityFromEmail(email);
            var validRefreshToken = _authService.VerifyRefreshToken(user, tokens.RefreshToken, applicationClient);
            if (!validRefreshToken)
                return new UnauthorizedResult();

            var newJwtToken = _tokenService.GenerateTokenFor(user);
            var newRefreshTokenString = _tokenService.GenerateRefreshToken();
            var newRefreshToken = _tokenService.GetRefreshTokenObject(newRefreshTokenString);
            await _authService.UpdateRefreshToken(user, newRefreshToken, applicationClient);

            var expirationDate = new DateTimeOffset(newRefreshToken.ExpirationDate).ToUnixTimeSeconds();

            return new ObjectResult(new Tokens
            {
                Token = newJwtToken,
                RefreshToken = newRefreshTokenString,
                ExpirationDate = expirationDate
            });
        }

        public class Tokens
        {
            public string Token { get; set; }
            public string RefreshToken { get; set; }
            public long ExpirationDate { get; set; }
        }
    }
}
