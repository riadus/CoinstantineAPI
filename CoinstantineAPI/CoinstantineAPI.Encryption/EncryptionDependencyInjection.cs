using CoinstantineAPI.Core.Encryption;
using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.Encryption
{
    public static class EncryptionDependencyInjection
    {
        public static IServiceCollection AddEncryptionServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPasswordProvider, PasswordProvider>();
            serviceCollection.AddSingleton<IEncryptorProvider, EncryptorProvider>();
            serviceCollection.AddSingleton<ICryptoService, CryptoService>();
            serviceCollection.AddSingleton<IPrivateKeyGenerator, PrivateKeyGenerator>();

            return serviceCollection;
        }
    }
}
