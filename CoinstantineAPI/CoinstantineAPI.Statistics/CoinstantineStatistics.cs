using System;
using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Statistics;
using CoinstantineAPI.Core.Users;
using Microsoft.EntityFrameworkCore;

namespace CoinstantineAPI.Statistics
{
    public class CoinstantineStatistics : ICoinstantineStatistics
    {
        public int Subscriptions { get; set; }
        public int Valids { get; set; }
        public int TwitterValidations { get; set; }
        public int BctValidations { get; set; }
        public int TelegramValidations { get; set; }
        public int DiscordValidations { get; set; }

        private DateTime _lastCheck;
        private readonly IContextProvider _contextProvider;
        private readonly IUsersService _usersService;

        public CoinstantineStatistics(IContextProvider contextProvider, IUsersService usersService)
        {
            _contextProvider = contextProvider;
            _usersService = usersService;
        }

        public string Print()
        {
            return $"Last check at {_lastCheck.ToString("g")}\nNumber of subscriptions : {Subscriptions}\n" +
                $"Number of valid subscriptions : {Valids}\n" +
                $"Number of Twitter validated profiles : {TwitterValidations}\n" +
                $"Number of BCT validated profiles : {BctValidations}\n" +
                $"Number of Telegram validated profiles : {TelegramValidations}\n" +
                $"Number of Discord validated profiles : {DiscordValidations}";
        }

        public async Task<ICoinstantineStatistics> Check()
        {
            if ((DateTime.Now - _lastCheck).TotalHours >= 1)
            {
                await DoCheck();
            }
            return this;
        }

        private async Task DoCheck()
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                Subscriptions = await context.UserIdentities.CountAsync();
                Valids = await context.ApiUsers.CountAsync();
                TwitterValidations = await context.TwitterProfiles.CountAsync();
                BctValidations = await context.BitcoinTalkProfiles.CountAsync();
                TelegramValidations = await context.TelegramProfiles.CountAsync();
                DiscordValidations = await context.DiscordProfiles.CountAsync();
            }
            _lastCheck = DateTime.Now;
        }

        public async Task<ICoinstantineStatistics> ForceCheck(string callerId)
        {
            var user = await _usersService.GetUserFromDiscord(callerId, "Coinstantine");
            if (user?.Username != "Admin")
            {
                return null;
            }
            await DoCheck();
            return this;
        }
    }
}
