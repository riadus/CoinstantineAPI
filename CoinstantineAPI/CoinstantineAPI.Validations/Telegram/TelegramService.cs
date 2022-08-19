using System;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.Core.Extensions;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Core.Validations;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Users
{
    public class TelegramService : ITelegramService
    {
        private readonly ITelegramInfoProvider _telegramInfoProvider;
        private readonly IUsersService _usersService;
        private readonly IContextProvider _contextProvider;

        public TelegramService(ITelegramInfoProvider telegramInfoProvider,
                               IUsersService usersService,
                               IContextProvider contextProvider)
        {
            _telegramInfoProvider = telegramInfoProvider;
            _usersService = usersService;
            _contextProvider = contextProvider;
        }

        public async Task<(IProfileItem Profile, bool Success)> Cancel(ApiUser user)
        {
            try
            {
                var profile = user.Telegram;
                if (profile.ValidationDate >= SystemTime.Now().AddMinutes(-2))
                {
                    var successfullyRemoved = await _usersService.RemoveTelegramProfile(user);
                    return (null, successfullyRemoved);
                }
                return (profile, false);
            }
            catch
            {
                return (null, false);
            }
        }

        public async Task<IProfileItem> GetProfileItem(string id)
        {
            var profile = _telegramInfoProvider.GetTelegramProfile(id);
            return profile.TelegramProfile;
        }

        public bool IsTheUser(ApiUser user, string id)
        {
            return user.Telegram.Username.ToLower() == id.ToLower();
        }

        public async Task<(IProfileItem Profile, bool Success)> SetProfileItem(ApiUser user, string id, object encapsulatedData = null)
        {
            var (telegramProfile, phonenumber) = _telegramInfoProvider.GetTelegramProfile(id, true);
            telegramProfile.Validated = true;
            telegramProfile.ValidationDate = DateTime.Now;
            user.Telegram = telegramProfile;
            user.Phonenumber = phonenumber;
            var result = await _usersService.SetTelegramProfile(user);
            return (telegramProfile, result.AllGood);
        }

        public async Task StartProcess(string id)
        {
            await _telegramInfoProvider.ProcessConversationOnTelegram(id);
        }

        public Task<IProfileItem> Update(ApiUser user, string id)
        {
            throw new NotSupportedException("Telegram can't be updated");
        }

        public bool HasValidatedProfile(ApiUser user)
        {
            return user.Telegram?.Validated ?? false;
        }
    }
}
