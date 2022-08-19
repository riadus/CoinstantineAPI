using CoinstantineAPI.Core.Statistics;
using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.Statistics
{
    public static class StatisticsDependencyInjection
    {
        public static IServiceCollection AddStatisticsServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IStatisticsProvider, StatisticsProvider>();
            serviceCollection.AddSingleton<ICoinstantineStatistics, CoinstantineStatistics>();
            return serviceCollection;
        }
    }
}
