using AutoMapper;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Core.Validations;
using CoinstantineAPI.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/bitcoinTalk")]
    public class BitcoinTalkController : ThirdPartyValidationController<long, BitcoinTalkProfileResponse>
    {
        public BitcoinTalkController(IUsersService userService,
                                     IMapper mapper,
                                     IBitcoinTalkService bitcoinTalkService,
                                     ILoggerFactory loggerFactory,
                                     IUserResolverService userResolverService) : base(userService, mapper, bitcoinTalkService, loggerFactory, userResolverService)
        {
        }
    }
}
