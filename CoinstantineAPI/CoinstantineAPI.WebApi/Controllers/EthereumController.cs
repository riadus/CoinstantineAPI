using System.Threading.Tasks;
using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Core.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoinstantineAPI.Core.Extensions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/ethereum")]
    [Authorize]
    public class EthereumController : BaseController
    {
        private readonly IEthereumService _ethereumService;

        public EthereumController(IUsersService usersService,
                                  IEthereumService ethereumService,
                                  ILoggerFactory loggerFactory,
                                  IUserResolverService userResolverService) : base(usersService, loggerFactory, userResolverService)
        {
            _ethereumService = ethereumService;
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendBalanceTo([FromBody] SendDTO sendDTO)
        {
            var blockchainUser = await GetBlockchainUser();
            var user = await GetApiUser();
            if(blockchainUser.Address != sendDTO?.FromAddress || user.BctProfile?.Location == null || user.BctProfile.Location != sendDTO.ToAddress)
            {
                return BadRequest();
            }


            var receipt = await _ethereumService.SendFunds(blockchainUser, sendDTO.ToAddress);
            return receipt.IsNotNull() ? Ok(new WithdrawalReceipt { TransactionHash = receipt }) : (IActionResult)BadRequest();
        }

        [HttpGet]
        [Route("price")]
        public async Task<IActionResult> GetGasPrice()
        {
            return Ok(await _ethereumService.GetGasPrice());
        }

        public class SendDTO
        {
            public string ToAddress { get; set; }
            public string FromAddress { get; set; }
            public decimal Eth { get; set; }
        }

        public class WithdrawalReceipt
        {
            public string TransactionHash { get; set; }
        }
    }
}
