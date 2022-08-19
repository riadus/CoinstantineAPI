using CoinstantineAPI.Core.Games;
using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.Games
{
    public static class GamesDependencyInjection
    {
        public static IServiceCollection AddGameServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IGameService, GameService>();
            serviceCollection.AddTransient<IBountyProgram, BountyProgram>();
            return serviceCollection;
        }
    }
}
