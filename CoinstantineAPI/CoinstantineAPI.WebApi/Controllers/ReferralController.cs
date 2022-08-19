using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoinstantineAPI.Core.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoinstantineAPI.WebApi.DTO;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/referral")]
    [Authorize]
    public class ReferralController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IReferralService _referralService;

        public ReferralController(IUsersService userService,
                                  ILoggerFactory loggerFactory,
                                  IMapper mapper,
                                  IUserResolverService userResolverService,
                                  IReferralService referralService) : base(userService, loggerFactory, userResolverService)
        {
            _mapper = mapper;
            _referralService = referralService;
        }

        [HttpGet("link")]
        public async Task<IActionResult> GetLink()
        {
            var user = await GetApiUser();
            var link = await _referralService.GetReferralLink(user);
            return Ok(_mapper.Map<ReferralResponse>(link)); ;
        }

        [HttpGet]
        public async Task<IActionResult> GetReferrals()
        {
            var user = await GetApiUser();
            var users = await _referralService.GetReferrals(user);
            return Ok(users?.Select(x => new ReferralUserResponse
            {
                Username = x.Username,
                CreationDate = x.UserIdentity.SubscriptionDate
            }) ?? new List<ReferralUserResponse>());
        }
    }
}
