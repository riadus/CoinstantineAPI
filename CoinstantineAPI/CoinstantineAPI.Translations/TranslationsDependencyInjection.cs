using CoinstantineAPI.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.Translations
{
    public static class TranslationsDependencyInjection
    {
        public static IServiceCollection AddTranslations(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ITranslationService, TranslationSevice>();

            return serviceCollection;
        }
    }
}
