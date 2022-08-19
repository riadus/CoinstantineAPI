using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Core.Validations;
using CoinstantineAPI.Users;
using FakeItEasy;

namespace CoinstantineAPI.Tests.Builders
{
    public class BitcoinTalkServiceBuilder
    {
        private IBitcoinTalkPublicProfileProvider _bitcoinTalkPublicProfileProvider = A.Fake<IBitcoinTalkPublicProfileProvider>();
        private IUsersService _usersService = A.Fake<IUsersService>();
        private IContextProvider _contextProvider = A.Fake<IContextProvider>();

        public IBitcoinTalkService Build()
        {
            return new BitcoinTalkService(_bitcoinTalkPublicProfileProvider, _usersService, _contextProvider);
        }

        public BitcoinTalkServiceBuilder WithBitcoinTalkPublicProfileProvider(IBitcoinTalkPublicProfileProvider bitcoinTalkPublicProfileProvider)
        {
            _bitcoinTalkPublicProfileProvider = bitcoinTalkPublicProfileProvider;
            return this;
        }

        public BitcoinTalkServiceBuilder WithContextProvider(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
            return this;
        }

        public BitcoinTalkServiceBuilder WithUsersService(IUsersService usersService)
        {
            _usersService = usersService;
            return this;
        }
    }
}
