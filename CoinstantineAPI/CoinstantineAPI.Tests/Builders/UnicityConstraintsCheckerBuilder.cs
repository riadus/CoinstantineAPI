using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Users.Unicity;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Tests.Builders
{
    public class UnicityConstraintsCheckerBuilder
    {
        private IUnicityConstraintsFactory _unicityConstraintsFactory = A.Dummy<IUnicityConstraintsFactory>();
        private IContextProvider _contextProvider = A.Dummy<IContextProvider>();
        private ILoggerFactory _loggerFactory = A.Dummy<ILoggerFactory>();

        public IUnicityConstraintsChecker Build()
        {
            return new UnicityConstraintsChecker(_unicityConstraintsFactory, _contextProvider, _loggerFactory);
        }

        public IUnicityConstraintsChecker BuildForIntegration(IContextProvider contextProvider)
        {
            return new UnicityConstraintsChecker(new UnicityConstraintsFactory(), contextProvider, _loggerFactory);
        }

        public UnicityConstraintsCheckerBuilder WithContextProvider(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
            return this;
        }

        public UnicityConstraintsCheckerBuilder WithUnicityConstraintsFactory(IUnicityConstraintsFactory unicityConstraintsFactory)
        {
            _unicityConstraintsFactory = unicityConstraintsFactory;
            return this;
        }
    }
}
