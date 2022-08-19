using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.TwitterProvider;
using Microsoft.Extensions.DependencyInjection;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models;

namespace CoinstantineAPI.DataProvider.TwitterProvider
{
    public static class TwitterProviderDependencyInjection
    {
        public static IServiceCollection AddTwitterProvider(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ITwitterInfoProvider, TwitterInfoProvider>(); 
            serviceCollection.AddSingleton<IRandomTweetsProvider, RandomTweetsProvider>(); 
            JsonPropertyConverterRepository.JsonConverters.Remove(typeof(Language));
            JsonPropertyConverterRepository.JsonConverters.Add(typeof(Language), new CustomJsonLanguageConverter());
            return serviceCollection;
        }
    }
}
