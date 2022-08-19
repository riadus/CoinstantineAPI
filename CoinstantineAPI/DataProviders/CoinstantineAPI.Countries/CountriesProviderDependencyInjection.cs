using CoinstantineAPI.Core.DataProviders;
using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.Countries
{
    public static class CountriesProviderDependencyInjection
    {
        public static IServiceCollection AddCountriesProvider(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICountriesProvider, CountriesProvider>();
            return serviceCollection;
        }
    }
}
