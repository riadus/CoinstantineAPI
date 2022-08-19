using System.Collections.Concurrent;
using System.Threading.Tasks;
using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using CoinstantineAPI.TelegramProvider;
using CoinstantineAPI.TelegramProvider.Entities;

namespace CoinstantineAPI.DataProvider.TelegramProvider
{
    public class TelegramInfoProvider : ITelegramInfoProvider
    {
        private readonly ITelegramBotManager _telegramBotManager;
        private readonly IUsersService _usersService;
        private readonly ConcurrentDictionary<string, (TelegramProfile, string)> _telegramProfiles;

        public TelegramInfoProvider(ITelegramBotManager telegramBot, IUsersService usersService)
        {
            _telegramBotManager = telegramBot;
            _usersService = usersService;
            _telegramProfiles = new ConcurrentDictionary<string, (TelegramProfile, string)>();
        }

        private void AddTelegramProfile(AppUpdate update)
        {
            var contact = update.Message.Contact;
            var from = update.Message.From;
            var telegramProfile = new TelegramProfile
            {
                TelegramId = contact.UserId,
                Username = from.Username,
                FirstName = from.FirstName,
                LastName = from.LastName
            };
            if (!_telegramProfiles.ContainsKey(telegramProfile.Username?.ToLower()))
            {
                _telegramProfiles.TryAdd(telegramProfile.Username.ToLower(), (telegramProfile, contact.PhoneNumber));
            }
        }

        public Task ProcessConversationOnTelegram(string username)
        {
            return _telegramBotManager.StartListeningForUser(username, AddTelegramProfile);
        }

        public (TelegramProfile TelegramProfile, string Phonenumber) GetTelegramProfile(string username, bool dispose)
        {
            if (_telegramProfiles.ContainsKey(username?.ToLower()))
            {
                var telegramInfo = _telegramProfiles[username.ToLower()];
                if (dispose)
                {
                    _telegramProfiles.TryRemove(username.ToLower(), out var value);
                }
                return telegramInfo;
            }
            return (null, null);
        }
    }
}