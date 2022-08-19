using CoinstantineAPI.Blockchain.Web3;
using CoinstantineAPI.Core.Blockchain;
using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.Blockchain
{
    public static class BlockchainDependencyInjection
    {
        public static IServiceCollection AddBlockchainServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IPrivateKeyRetreiver, PrivateKeyRetreiver>();
            serviceCollection.AddSingleton<IPresaleService, PresaleService>();
            serviceCollection.AddTransient<IEthereumService, EthereumService>();
            serviceCollection.AddTransient<ICoinstantineService, CoinstantineService>();
            serviceCollection.AddTransient<IBlockchainService, BlockchainService>();
            serviceCollection.AddSingleton<IWeb3Provider, Web3Provider>();
            serviceCollection.AddSingleton<IWeb3AccountService, Web3AccountService>();
            serviceCollection.AddSingleton<ISmartContractProvider, SmartContractProvider>();
            serviceCollection.AddSingleton<ISmartContractFactory, SmartContractFactory>();
            serviceCollection.AddTransient<ISmartContractExecutor, SmartContractExecutor>();
            serviceCollection.AddTransient<ICoinstantineSmartContractExecutor, CoinstantineSmartContractExecutor>();

            return serviceCollection;
        }
    }
}