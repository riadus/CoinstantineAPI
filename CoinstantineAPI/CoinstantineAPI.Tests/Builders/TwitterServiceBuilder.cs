using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Core.Validations;
using CoinstantineAPI.Users;
using FakeItEasy;

namespace CoinstantineAPI.Tests.Builders
{
    public class TwitterServiceBuilder
    {
        private ITwitterInfoProvider _twitterInfoProvider = A.Fake<ITwitterInfoProvider>();
        private IUsersService _usersService = A.Fake<IUsersService>();

        public ITwitterService Build()
        {
            return new TwitterService(_twitterInfoProvider, _usersService);
        }

        public TwitterServiceBuilder WithTwitterInfoProvider(ITwitterInfoProvider twitterInfoProvider)
        {
            _twitterInfoProvider = twitterInfoProvider;
            return this;
        }

        public TwitterServiceBuilder WithUsersService(IUsersService usersService)
        {
            _usersService = usersService;
            return this;
        }
    }
}
