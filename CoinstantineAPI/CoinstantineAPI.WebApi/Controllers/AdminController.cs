using System.Threading.Tasks;
using AutoMapper;
using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.DiscordBot;
using CoinstantineAPI.TwitterProvider;
using CoinstantineAPI.WebApi.Middleware.Attributes;
using CoinstantineAPI.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/admin")]
    [AuthorizeAdminOnly(AuthenticationSchemes = "BasicAuthentication")]
    public class AdminController : BaseController
    {
        private readonly IUserCreationService _userCreationService;
        private readonly IDiscordBot _discordBot;
        private readonly IMapper _mapper;
        private readonly IRandomTweetsProvider _randomTweetsProvider;
        private readonly IBlockchainService _blockchainService;

        public AdminController(IUserCreationService userCreationService,
                               IDiscordBot discordBot,
                               IMapper mapper,
                               IUsersService usersService,
                               IRandomTweetsProvider randomTweetsProvider,
                               IBlockchainService blockchainService,
                               ILoggerFactory loggerFactory,
                               IUserResolverService userResolverService) : base(usersService, loggerFactory, userResolverService)
        {

            _userCreationService = userCreationService;
            _discordBot = discordBot;
            _mapper = mapper;
            _randomTweetsProvider = randomTweetsProvider;
            _blockchainService = blockchainService;
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserProfileForAdmin([FromQuery]string email)
        {
            _logger.LogInformation("Getting userprofile");
            var apiUser = await GetApiUserFromEmail(email);

            if (apiUser != null)
            {
                _logger.LogInformation("Returning apiUser for getUserProfile()");
                return Ok(_mapper.Map<ApiUserResponse>(apiUser));
            }
            return BadRequest();
        }

        [HttpPost("twitter")]
        public async Task<IActionResult> ForceLoadTweets()
        {
            _logger.LogInformation("Force loading Tweets");
            await _randomTweetsProvider.ForceReload();
            return Ok();
        }

        [HttpPost("discord")]
        public async Task<IActionResult> WakeUpDiscord()
        {
            _logger.LogInformation("Waking up Discord");
            await _discordBot.InitializeAsync();
            return Ok();
        }
    }
}
