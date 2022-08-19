using System.Threading.Tasks;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Core.Validations;
using CoinstantineAPI.WebApi.DTO;
using CoinstantineAPI.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/twitter")]
    public class TwitterController : ThirdPartyValidationController<long, TwitterProfileResponse>
    {
        private readonly ISingletonTwitterService _singletonTwitterService;

        public TwitterController(IUsersService userService,
                                 AutoMapper.IMapper mapper,
                                 ITwitterService twitterService,
                                 ISingletonTwitterService singletonTwitterService,
                                 ILoggerFactory loggerFactory,
                                 IUserResolverService userResolverService) : base(userService, mapper, twitterService, loggerFactory, userResolverService)
        {
            _singletonTwitterService = singletonTwitterService;
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomTweet([FromQuery] string language)
        {
            var tweet = await _singletonTwitterService.GetRandomTweet(language);
            if(tweet == null)
            {
                return NoContent();
            }
            return Ok(tweet);
        }

        [HttpGet("referral")]
        public async Task<IActionResult> GetListOfTweetsForReferral([FromQuery] string language)
        {
            var tweets = await _singletonTwitterService.GetListOfTweetsForReferral(language);
            if (tweets == null)
            {
                return NoContent();
            }
            return Ok(tweets);
        }

        [HttpGet("config")]
        public async Task<IActionResult> GetTwitterConfig([FromQuery] string language)
        {
            var tweet = await _singletonTwitterService.GetRandomTweet(language);
            if (tweet == null)
            {
                return NoContent();
            }
            var config = await _singletonTwitterService.GetTwitterConfig();
            if(config == null)
            {
                return NoContent();
            }
            var configResponse = _mapper.Map<TwitterConfigResponse>(config);
            var tweetResposne = _mapper.Map<TweetResponse>(tweet);
            configResponse.TweetTextToTweet = tweetResposne;
            return Ok(configResponse);
        }
    }
}
