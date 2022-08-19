using CoinstantineAPI.Core.External;
using CoinstantineAPI.Scan.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.Scan
{
    public static class ScanDependencyInjection
    {
        public static IServiceCollection AddScanServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IPriceService, PriceService>();
            serviceCollection.AddTransient<ICryptoCompare, CryptoCompare>();
            serviceCollection.AddTransient<IApiClientFactory, ApiClientFactory>();

            return serviceCollection;
        }
    }
}
