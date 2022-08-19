using System.Threading.Tasks;
using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Core.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoinstantineAPI.WebApi.Controllers
{
    //[Route("api/airdrop/users")]
    [Authorize]
    public class AirdropUsersController : BaseController
    {
        private ICoinstantineService _airdropService;
        public AirdropUsersController(IUsersService usersService,
                                      ICoinstantineService airdropService,
                                      ILoggerFactory loggerFactory,
                                      IUserResolverService userResolverService) : base(usersService, loggerFactory, userResolverService)
        {
            _airdropService = airdropService;
        }

        //[HttpPost]
        [Route("{userId}/{airdropId}")]
        private async Task<IActionResult> Subcribe(string userId, string airdropId)
        {
            return BadRequest();
            /*
            var success = await _airdropService.Subscribe(userId, airdropId);
            if (!success)
                return BadRequest();
            return Ok();*/
        }

        //[HttpPost]
        [Route("{userId}/{airdropId}/withdraw/")]
        private async Task<IActionResult> Withdraw(string userId, string airdropId)
        {
            return BadRequest();
            /*
            var distributed = await _airdropService.Withdraw(airdropId, userId);
            return Ok(distributed);*/
        }
    }
}
