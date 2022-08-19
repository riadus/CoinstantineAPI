using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CoinstantineAPI.Blockchain.Web3;
using CoinstantineAPI.Core;
using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using CoinstantineAPI.WebApi.DTO;
using CoinstantineAPI.WebApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/presale")]
    [Authorize]
    public class PresaleController : BaseController
    {
        private readonly IBlockchainService _blockchainService;
        private readonly IPresaleService _presaleService;
        private readonly IWeb3Provider _web3Provider;
        private readonly IMapper _mapper;
        private readonly IContextProvider _contextProvider;

        public PresaleController(IUsersService usersService,
                                 IBlockchainService blockchainService,
                                 IPresaleService presaleService,
                                 IWeb3Provider web3Provider,
                                 IMapper mapper,
                                 IContextProvider contextProvider,
                                 ILoggerFactory loggerFactory,
                                 IUserResolverService userResolverService) : base(usersService, loggerFactory, userResolverService)
        {
            _blockchainService = blockchainService;
            _presaleService = presaleService;
            _web3Provider = web3Provider;
            _mapper = mapper;
            _contextProvider = contextProvider;
        }

        [HttpGet]
        [Route("rate")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRate()
        {
            return Ok(await _presaleService.GetCurrentRate());
        }

        [HttpGet]
        [Route("raised")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAmountRaised()
        {
            return Ok(await _presaleService.GetAmountRaised());
        }

        [HttpPost]
        [Route("buy")]
        public async Task<IActionResult> Buy([FromBody] BuyTokensDTO buyTokensDTO)
        {
            var blockchainUser = await GetBlockchainUser();
            var user = await GetApiUser();
            if(blockchainUser.Address != buyTokensDTO.Address)
            {
                return BadRequest();
            }
            var buyingResult = await _presaleService.Buy(blockchainUser, buyTokensDTO.Amount, user.Email);
            if(buyingResult?.BuyerId == blockchainUser.Id)
            {
                return Ok(new Purchase
                {
                    ActualPurchase = _mapper.Map<BuyTokensResultResponse>(buyingResult)
                });
            }
            return BadRequest();
        }

        public class Purchase
        {
            public BuyTokensResultResponse ActualPurchase { get; set; }
        }

        [HttpGet]
        [Route("purchases")]
        public async Task<IActionResult> GetAllPurchases()
        {
            var user = await GetBlockchainUser();
            var purchases = await _presaleService.GetPurchases(user);
            var response = _mapper.Map<IEnumerable<BuyTokensResultResponse>>(purchases);
            return Ok(response);
        }

        [HttpPost]
        [Route("claimBack")]
        public async Task<IActionResult> ClaimBack([FromBody] BuyTokensDTO buyTokensDTO)
        {
            var user = await GetBlockchainUser();
            if (user.Address != buyTokensDTO.Address)
            {
                return BadRequest();
            }
            return Ok(await _presaleService.ClaimBack(user));
        }

        [HttpPost]
        [Route("deploy")]
        public async Task<IActionResult> SaveDeployedContract([FromBody] SmartContract smartContract)
        {
            return Ok(await _blockchainService.DeployContract(smartContract));
        }

        [HttpGet]
        [Route("buyers")]
        public async Task<IActionResult> GetBuyers()
        {
            return Ok(await _presaleService.GetBuyers());
        }

        [HttpGet]
        [Route("web3")]
        public IActionResult GetWeb3Url()
        {
            return Ok(new EnvironmentResponse
            {
                Environment = Constants.ApiEnvironment,
                Web3Url = _web3Provider.Url,
                EtherscanUrl = Constants.EtherscanUrl,
                EthereumEnvironment = Constants.EthereumEnvironment
            });
        }

        [HttpGet]
        [Route("smartContractDefinition/{name}")]
        public async Task<IActionResult> GetSmartContract(string name)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                var smartContract = await context.SmartContracts
                                                 .Include(s => s.Token)
                                                 .FirstOrDefaultAsync(x => x.Name == name);
                return Ok(_mapper.Map<SmartContractResponse>(smartContract));
            }
        }

        [HttpGet]
        [Route("details/{transactionHash}")]
        public async Task<IActionResult> GetTransactionReceipt(string transactionHash)
        {
            return Ok(await _presaleService.GetReceipt(transactionHash));
        }
    }
}
