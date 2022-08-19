using System.Threading.Tasks;
using AutoMapper;
using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using CoinstantineAPI.WebApi.DTO;
using CoinstantineAPI.WebApi.Middleware.Attributes;
using CoinstantineAPI.WebApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/users")]
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserCreationService _userCreationService;
        private readonly IMapper _mapper;
        private readonly IBlockchainService _blockchainService;

        public UsersController(IUserCreationService userCreationService,
                               IMapper mapper,
                               IUsersService usersService,
                               IBlockchainService blockchainService,
                               ILoggerFactory loggerFactory,
                               IUserResolverService userResolverService) : base(usersService, loggerFactory, userResolverService)
        {

            _userCreationService = userCreationService;
            _mapper = mapper;
            _blockchainService = blockchainService;
        }

        [HttpPost]
        [Route("username")]
        public async Task<IActionResult> SetUsername([FromBody] ApiUserDTO apiUserDto)
        {
            _logger.LogInformation("Setting username", apiUserDto.Username);
            AzureUser user = GetUser();

            var (apiUser, success) = await _userCreationService.CreateApiUser(user, apiUserDto.Username);
            if (success)
            {
                _logger.LogInformation("Returning apiUser");
                return Ok(_mapper.Map<ApiUserResponse>(apiUser));
            }
            _logger.LogError("Couldn't create apiUser");
            return BadRequest("User already exists");
        }

        [HttpGet]
        public async Task<IActionResult> GetUserProfile()
        {
            _logger.LogInformation("Getting userprofile");
            var user = GetUser();
            var apiUser = await GetApiUser();

            if (apiUser != null)
            {
                _logger.LogInformation("Returning apiUser for getUserProfile()");
                return Ok(_mapper.Map<ApiUserResponse>(apiUser));
            }
            return BadRequest();
        }


    }
}