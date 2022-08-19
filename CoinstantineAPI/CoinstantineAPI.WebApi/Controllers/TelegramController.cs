using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Core.Validations;
using CoinstantineAPI.TelegramProvider;
using CoinstantineAPI.TelegramProvider.Entities;
using CoinstantineAPI.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/telegram")]
    public class TelegramController : ThirdPartyValidationController<string, TelegramProfileResponse>
    {
        private readonly ITelegramService _telegramService;

        public TelegramController(IUsersService userService,
                                  IMapper mapper,
                                  ITelegramService telegramService,
                                  ILoggerFactory loggerFactory,
                                  IUserResolverService userResolverService) : base(userService, mapper, telegramService, loggerFactory, userResolverService)
        {
            _telegramService = telegramService;
        }

        [HttpPost]
        [Route("{username}/startConversation")]
        public async Task<IActionResult> StartConversation(string username)
        {
            var user = await GetApiUser();
            await Task.Factory.StartNew(() => _telegramService.StartProcess(username));
            return Ok();
        }
    }

    [Route("api/telegramWebhook")]
    public class TelegramWebhookController : Controller
    {
        private readonly ITelegramBotManager _telegramBotManager;

        public TelegramWebhookController(ITelegramBotManager telegramBotManager)
        {
            _telegramBotManager = telegramBotManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AppUpdate update)
        {
            await _telegramBotManager.HandleUpdate(update);
            return Ok();
        }

        [HttpPost]
        [Route("start")]
        public async Task<IActionResult> Start()
        {
            await _telegramBotManager.StartListening();
            return Ok();
        }
    }
}
