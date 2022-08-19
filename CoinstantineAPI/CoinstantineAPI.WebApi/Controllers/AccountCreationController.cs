using System;
using System.Threading.Tasks;
using AutoMapper;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using CoinstantineAPI.VerifyCaptcha;
using CoinstantineAPI.WebApi.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/create-account")]
    public class AccountCreationController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUserCreationService _userCreationService;
        private readonly IReCaptchaValidator _reCaptchaValidator;

        public AccountCreationController(IUsersService userService,
                                         IMapper mapper,
                                         IUserCreationService userCreationService,
                                         ILoggerFactory loggerFactory,
                                         IUserResolverService userResolverService,
                                         IReCaptchaValidator reCaptchaValidator) : base(userService, loggerFactory, userResolverService)
        {
            _mapper = mapper;
            _userCreationService = userCreationService;
            _reCaptchaValidator = reCaptchaValidator;
        }

        [HttpPost("check")]
        public async Task<IActionResult> CheckAccount([FromBody] AccountCreationModel accountCreationModel)
        {
            return Ok(await _userCreationService.IsAccountCorrect(accountCreationModel));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] AccountCreationModel accountCreationModel)
        {
            var reCaptchaResponse = await _reCaptchaValidator.Validate(Request.Headers);
            if (!reCaptchaResponse.Success)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            var accountCorrect = await _userCreationService.IsAccountCorrect(accountCreationModel);
            if (!accountCorrect.AllGood)
            {
                return Ok(accountCorrect);
            }

            if (await _userCreationService.SubscribeUser(accountCreationModel))
            {
                return Ok(accountCorrect);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("confirm/{userId}")]
        public async Task<IActionResult> Confirm([FromRoute] string userId, [FromQuery] string confirmationCode)
        {
            var user = await _userCreationService.GetUserFromConfirmationCode(confirmationCode);
            if (user.UserId != userId)
            {
                return BadRequest();
            }
            var success = await _userCreationService.CreateApiUser(user);
            if (success)
            {
                _logger.LogInformation("Returning apiUser");
                return Ok();
            }
            _logger.LogError("Couldn't create apiUser");
            return StatusCode(StatusCodes.Status409Conflict);
        }

        [HttpPost]
        [Route("reset/username")]
        public async Task<IActionResult> ResetUsername([FromQuery]string email)
        {
            var reCaptchaResponse = await _reCaptchaValidator.Validate(Request.Headers);
            if (!reCaptchaResponse.Success)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            if (await _userCreationService.SendUsername(email))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("reset/password")]
        public async Task<IActionResult> ResetPassword([FromQuery]string email)
        {
            var reCaptchaResponse = await _reCaptchaValidator.Validate(Request.Headers);
            if (!reCaptchaResponse.Success)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            if (await _userCreationService.ResetPassword(email))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("change/password/{userId}")]
        public async Task<IActionResult> ChangePassword([FromRoute] string userId, [FromQuery] string confirmationCode, [FromBody] AccountChangeViewModel accountChangeViewModel)
        {
            if (!_userCreationService.CorrectPassword(accountChangeViewModel.Password))
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            var accountChangeModel = _mapper.Map<AccountChangeModel>(accountChangeViewModel);
            accountChangeModel.UserId = userId;
            accountChangeModel.ConfirmationCode = confirmationCode;
            try
            {
                if (await _userCreationService.ChangePassword(accountChangeModel))
                {
                    return Ok();
                }
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }
    }
}
