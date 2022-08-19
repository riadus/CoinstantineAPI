using CoinstantineAPI.Aidrops.Requirements;
using CoinstantineAPI.Aidrops.Requirements.Interfaces;

namespace CoinstantineAPI.Tests.Builders
{
    public class RequirementToLambdaBuilder
    {
        public IRequirementToLambda Build()
        {
            return new RequirementToLambda();
        }
    }
}
