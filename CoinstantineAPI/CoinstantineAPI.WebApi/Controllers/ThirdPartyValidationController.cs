using System;
using System.Threading.Tasks;
using AutoMapper;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Core.Validations;
using CoinstantineAPI.WebApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.WebApi.Controllers
{
    [Authorize]
    public class ThirdPartyValidationController<TEntity, TResponse> : BaseController
    {
        protected readonly IMapper _mapper;
        protected readonly IThirdPartyService<TEntity> _thirdPartyService;
        public ThirdPartyValidationController(IUsersService usersService,
                                              IMapper mapper,
                                              IThirdPartyService<TEntity> thirdPartyService,
                                              ILoggerFactory loggerFactory,
                                              IUserResolverService userResolverService) : base(usersService, loggerFactory, userResolverService)
        {
            _mapper = mapper;
            _thirdPartyService = thirdPartyService;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserProfileItem(TEntity userId)
        {
            try
            {
                var profileItem = await _thirdPartyService.GetProfileItem(userId);
                return Ok(_mapper.Map<TResponse>(profileItem));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("{userId}")]
        public async Task<IActionResult> SetUserProfileItem(TEntity userId, [FromBody] TwitterData encapsulatedData)
        {
            var user = await GetApiUser();
            try
            {
                if (_thirdPartyService.HasValidatedProfile(user))
                {
                    return Forbid();
                }
                var (profile, success) = await _thirdPartyService.SetProfileItem(user, userId, encapsulatedData);
                if (success)
                {
                    return Ok(_mapper.Map<TResponse>(profile));
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest(_mapper.Map<ApiUserResponse>(user));
            }
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<IActionResult> DeleteUserProfileItem(TEntity userId)
        {
            var user = await GetApiUser();
            try
            {
                if (!_thirdPartyService.IsTheUser(user, userId))
                {
                    return Forbid();
                }
                var (profile, success) = await _thirdPartyService.Cancel(user);
                if (success)
                {
                    return Ok();
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> UpdateUserProfileItem(TEntity userId)
        {
            var user = await GetApiUser();
            try
            {
                if (!_thirdPartyService.IsTheUser(user, userId))
                {
                    return Forbid();
                }
                var bctUser = await _thirdPartyService.Update(user, userId);
                return Ok(_mapper.Map<TResponse>(bctUser));
            }
            catch
            {
                return BadRequest(_mapper.Map<ApiUserResponse>(user));
            }
        }
    }
}
