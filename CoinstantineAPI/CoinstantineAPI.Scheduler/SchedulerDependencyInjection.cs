using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.Scheduler
{
    public static class SchedulerDependencyInjection
    {
        public static IServiceCollection AddSchedulerServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IScheduler, Scheduler>();
            return serviceCollection;
        }
    }
}
