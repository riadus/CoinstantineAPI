using CoinstantineAPI.Core.Documents;
using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.Documents
{
    public static class DocumentsProviderDependencyInjection
    {
        public static IServiceCollection AddDocumentProvider(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IDocumentInformationFactory, DocumentInformationFactory>();
            serviceCollection.AddSingleton<IDocumentProvider, DocumentProvider>();

            return serviceCollection;
        }
    }
}
