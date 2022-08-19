using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/airdrops")]
    [Authorize]
    public class AirdropsController : BaseController
    {
        private readonly ICoinstantineService _airdropService;
        public AirdropsController(ICoinstantineService airdropService,
                                  IUsersService usersService,
                                  ILoggerFactory loggerFactory,
                                  IUserResolverService userResolverService) : base(usersService, loggerFactory, userResolverService)
        {
            _airdropService = airdropService;
        }

        //[HttpPost]
        private async Task<IActionResult> CreateAirdrop([FromBody]Airdrop airdrop)
        {
            return BadRequest();
            /*if (airdrop == null) return BadRequest();
            var id = await _airdropService.CreateAirdrop(airdrop);
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            return Ok(id);*/
        }

        //[HttpPost]
        //[Route("{airdropId}/deposit/{amount}")]
        private async Task<IActionResult> Deposit(string airdropId, int amount)
        {
            return BadRequest();
            /*var success = await _airdropService.Deposit(airdropId, amount);
            if (success)
                return Ok();
            return BadRequest();*/
        }

        //[HttpGet]
        //[Route("{airdropId}/deposit/")]
        private async Task<IActionResult> Balance(string airdropId)
        {
            return BadRequest();
            /*var result = await _airdropService.CheckDeposit(airdropId);
            return Ok(result);*/
        }

        //[HttpGet]
        //[Route("{airdropId}/subscribers/")]
        private async Task<IActionResult> Subcribers(string airdropId)
        {
            return BadRequest();
            /*var users = await _airdropService.Subscribers(airdropId);
            if (!users?.Any() ?? true)
                return BadRequest();
            return Ok(users);*/
        }

        //[HttpPost]
        //[Route("{airdropId}/subscribers/")]
        private async Task<IActionResult> StartDistribtion(string airdropId)
        {
            return BadRequest();
            /*var distribution = await _airdropService.StartDistribution(airdropId);
            if (!distribution)
                return BadRequest();
            return Ok();*/
        }
    }
}
