using System.Threading.Tasks;
using AutoMapper;
using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Core.External;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using CoinstantineAPI.WebApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/sale")]
    [Authorize]
    public class SaleController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IPriceService _priceService;
        private readonly IBlockchainService _blockchainService;

        public SaleController(IUsersService usersService,
                              IMapper mapper,
                              IPriceService priceService,
                              IBlockchainService blockchainService,
                              ILoggerFactory loggerFactory,
                              IUserResolverService userResolverService) : base(usersService, loggerFactory, userResolverService)
        {
            _mapper = mapper;
            _priceService = priceService;
            _blockchainService = blockchainService;
        }

        [HttpGet]
        [Route("ether")]
        public async Task<IActionResult> GetEtherPrice()
        {
            return Ok(await _priceService.GetEtherPrice());
        }
            
        [HttpGet]
        [Route("balance")]
        public async Task<IActionResult> GetUserBalance()
        {
            var user = await GetApiUser();
            return Ok(_mapper.Map<BlockchainInfoResponse>(user.BlockchainInfo));
        }

        [HttpGet]
        [Route("balance/{address}")]
        public async Task<IActionResult> GetUserBalanceFor(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                var user = await GetApiUser();
                address = user.BlockchainInfo.Address;
            }
            var blockchainInfo = await _blockchainService.GetBalances(address);
            return Ok(_mapper.Map<BlockchainInfoResponse>(blockchainInfo));
        }
    }
}
