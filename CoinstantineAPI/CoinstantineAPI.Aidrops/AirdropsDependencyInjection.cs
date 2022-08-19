using CoinstantineAPI.Aidrops.Requirements;
using CoinstantineAPI.Aidrops.Requirements.Interfaces;
using CoinstantineAPI.Core.Airdrops;
using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.Aidrops
{
    public static class AirdropsDependencyInjection
    {
        public static IServiceCollection AddAirdropServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAirdropService, AirdropService>();
            serviceCollection.AddTransient<IRequirementToLambda, RequirementToLambda>();
            return serviceCollection;
        }
    }
}
