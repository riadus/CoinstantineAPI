using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.UnitTests;
using CoinstantineAPI.Users;
using CoinstantineAPI.Users.Unicity;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Tests.Builders
{
    public class UsersServiceBuilder
    {
        private IContextProvider _contextProvider = new ContextProviderBuilder().Build();
        private IUnicityConstraintsChecker _unityConstraintsChecker = A.Dummy<IUnicityConstraintsChecker>();
        private ILoggerFactory _loggerFactory = A.Dummy<ILoggerFactory>();

        public IUsersService Build()
        {
            return new UsersService(_contextProvider, A.Dummy<INotificationCenter>(), _unityConstraintsChecker, _loggerFactory);
        }

        public IUsersService BuildForIntegration()
        {
            var unicityConstraintsChecker = new UnicityConstraintsCheckerBuilder().BuildForIntegration(_contextProvider);
            return new UsersService(_contextProvider, A.Dummy<INotificationCenter>(), unicityConstraintsChecker, _loggerFactory);
        }

        public UsersServiceBuilder WithContextProvider(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
            return this;
        }

        public UsersServiceBuilder WithUnicityConstraintsChecker(IUnicityConstraintsChecker unityConstraintsChecker)
        {
            _unityConstraintsChecker = unityConstraintsChecker;
            return this;
        }
    }
}
