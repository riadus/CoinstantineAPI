using CoinstantineAPI.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.Notifications
{
    public enum Platform
    {
        iOS,
        Android
    }

    public static class NotificationsDependencyInjection
    {
        public static IServiceCollection AddNotifications(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IPayloadBuilderFactory, PayloadBuilderFactory>();
            serviceCollection.AddTransient<INotificationCenter, NotificationCenter>();

            return serviceCollection;
        }
    }
}
