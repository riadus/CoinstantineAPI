using System.Collections.Generic;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.WebApi.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/notification")]
    [AuthorizeAdminOnly(AuthenticationSchemes = "BasicAuthentication")]
    public class NotificationsController : BaseController
    {
        private readonly INotificationCenter _notificationCenter;

        public NotificationsController(IUsersService userService,
                                       INotificationCenter notificationCenter,
                                       IUserResolverService userResolverService,
                                       ILoggerFactory loggerFactory) : base(userService, loggerFactory, userResolverService)
        {
            _notificationCenter = notificationCenter;
        }

        [HttpGet]
        [Route("{email}")]
        public async Task<IActionResult> SendNotificationTo(string email)
        {
            try
            {
                await _notificationCenter.SendNotification("This is not a silent notification", email).ConfigureAwait(false);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendMessageNotificationTo([FromBody] MessageNotfication messageNotfication)
        {
            try
            {
                foreach (var recipient in messageNotfication.Recipients)
                {
                    await _notificationCenter.SendNotification(messageNotfication.Message, recipient).ConfigureAwait(false);
                }
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        public class MessageNotfication
        {
            public string Message { get; set; }
            public List<string> Recipients { get; set; }
        }

        [HttpGet]
        [Route("{email}/silent/{translationKey}")]
        public async Task<IActionResult> SendSilentNotificationTo(string email, string translationKey)
        {
            try
            {
                await _notificationCenter.SendSilentNotification(email, translationKey, PartToUpdate.Wallet).ConfigureAwait(false);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{email}/silent")]
        public async Task<IActionResult> SendSilentNotificationTo(string email)
        {
            try
            {
                await _notificationCenter.SendSilentNotification(email, null, PartToUpdate.Wallet).ConfigureAwait(false);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
