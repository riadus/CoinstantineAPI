using FakeItEasy;
using CoinstantineAPI.Aidrops;
using CoinstantineAPI.Aidrops.Requirements.Interfaces;
using CoinstantineAPI.Core.Airdrops;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.UnitTests;
using Microsoft.Extensions.Logging;
using CoinstantineAPI.Core.Games;

namespace CoinstantineAPI.Tests.Builders
{
    public class AirdropServiceBuilder
    {
        private IContextProvider _contextProvider = new ContextProviderBuilder().Build();
        private IRequirementToLambda _requirementToLambda = A.Fake<IRequirementToLambda>();
        private ILoggerFactory _loggerFactory = A.Dummy<ILoggerFactory>();
        private IBountyProgram _bountyProgram = A.Dummy<IBountyProgram>();

        public IAirdropService Build()
        {
            return new AirdropService(_contextProvider, _bountyProgram, _requirementToLambda, _loggerFactory);
        }

        public AirdropServiceBuilder WithContextProvider(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
            return this;
        }

        public AirdropServiceBuilder WithRequirementToLambda(IRequirementToLambda requirementToLambda)
        {
            _requirementToLambda = requirementToLambda;
            return this;
        }
    }
}
