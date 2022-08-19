using FakeItEasy;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.DataProvider.TelegramProvider;
using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.TelegramProvider;

namespace CoinstantineAPI.Tests.Builders
{
    public class TelegramInfoProviderBuilder
    {
        private ITelegramBotManager _telegramBotClient = A.Fake<ITelegramBotManager>();
        private IUsersService _usersServices = A.Fake<IUsersService>();
        public ITelegramInfoProvider Build()
        {
            return new TelegramInfoProvider(_telegramBotClient, _usersServices);
        }

        public TelegramInfoProviderBuilder WithTelegramBotClient(ITelegramBotManager telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
            return this;
        }

        public TelegramInfoProviderBuilder WithUsersServices(IUsersService usersServices)
        {
            _usersServices = usersServices;
            return this;
        }
    }
}

