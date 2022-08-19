using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Data;
using CoinstantineAPI.DataProvider.BitcoinTalkProvider;
using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.BitcoinTalkProvider
{
    public static class BitcoinTalkProviderDependencyInjection
    {
        public static IServiceCollection AddBitoinTalkProvider(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IBitcoinTalkPublicProfileProvider, BitcoinTalkPublicProfileProvider>();
            serviceCollection.AddSingleton<IMapper<string, BitcoinTalkRank>, BitcoinTalkRankMapper>();
            serviceCollection.AddSingleton<IMapper<BitcoinTalkUserDTO, BitcoinTalkProfile>, BitcoinTalkUserMapper>();

            return serviceCollection;
        }
    }
}
