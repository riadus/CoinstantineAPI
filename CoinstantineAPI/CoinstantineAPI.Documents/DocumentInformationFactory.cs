using System.Collections.Generic;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Documents;
using Microsoft.Extensions.FileProviders;
using Microsoft.WindowsAzure.Storage;

namespace CoinstantineAPI.Documents
{
    public class DocumentInformationFactory : IDocumentInformationFactory
    {
        private readonly Dictionary<DocumentType, IDocumentInformation> _documentInformations;
        public DocumentInformationFactory(IFileProvider fileProvider, CloudStorageAccount cloudStorageAccount, IContextProvider contextProvider)
        {
            _documentInformations = new Dictionary<DocumentType, IDocumentInformation>
            {
                { DocumentType.WhitePaper, new WhitePaperDocumentInformation(cloudStorageAccount, fileProvider, contextProvider)},
                { DocumentType.PrivacyPolicy, new PrivacyPolicyDocumentInformation(cloudStorageAccount, fileProvider, contextProvider)},
                { DocumentType.TermsAndServices, new TermsAndServicesDocumentInformation(cloudStorageAccount, fileProvider, contextProvider)}
            };
        }

        public async Task<IDocumentInformation> GetDocumentInformation(DocumentType documentType)
        {
            if (_documentInformations.ContainsKey(documentType))
            {
                var documentInformation = _documentInformations[documentType];
                await documentInformation.Init();
                return documentInformation;
            }
            return null;
        }
    }
}
