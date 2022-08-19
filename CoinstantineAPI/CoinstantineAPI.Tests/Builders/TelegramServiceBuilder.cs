using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Core.Validations;
using CoinstantineAPI.Users;
using FakeItEasy;

namespace CoinstantineAPI.Tests.Builders
{
    public class TelegramServiceBuilder
    {
        private ITelegramInfoProvider _telegramInfoProvider = A.Fake<ITelegramInfoProvider>();
        private IUsersService _usersService = A.Fake<IUsersService>();
        private IContextProvider _contextProvider = A.Fake<IContextProvider>();

        public ITelegramService Build()
        {
            return new TelegramService(_telegramInfoProvider, _usersService, _contextProvider);
        }

        public TelegramServiceBuilder WithTelegramInfoProvider(ITelegramInfoProvider telegramInfoProvider)
        {
            _telegramInfoProvider = telegramInfoProvider;
            return this;
        }

        public TelegramServiceBuilder WithContextProvider(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
            return this;
        }

        public TelegramServiceBuilder WithUsersService(IUsersService usersService)
        {
            _usersService = usersService;
            return this;
        }
    }
}
