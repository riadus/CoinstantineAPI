using System;
using System.Threading.Tasks;
using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.IntegrationTests.Mocks
{
    public class MockTelegramInfoProvider : ITelegramInfoProvider
    {
        public (TelegramProfile TelegramProfile, string Phonenumber) GetTelegramProfile(string username, bool dispose = false)
        {
            var id = Math.Abs(username.GetHashCode());
            var telegramProfile = new TelegramProfile
            {
                FirstName = username,
                LastName = username,
                Username = username,
                TelegramId = id
            };
            return (telegramProfile, $"31 {id}");
        }

        public Task ProcessConversationOnTelegram(string username)
        {
            return Task.FromResult(0);
        }
    }
}
