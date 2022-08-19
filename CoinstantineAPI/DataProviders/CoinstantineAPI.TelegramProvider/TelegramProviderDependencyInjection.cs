using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.TelegramProvider;
using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.DataProvider.TelegramProvider
{
    public static class TelegramProviderDependencyInjection
    {
        public static IServiceCollection AddTelegramProvider(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ITelegramBotClientMDT, TelegramBotClientMDT>();
            serviceCollection.AddSingleton<ITelegramInfoProvider, TelegramInfoProvider>();
            serviceCollection.AddSingleton<ITelegramBotManager, TelegramBotManager>();
            return serviceCollection;
        }
    }
}