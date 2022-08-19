using System.Collections.Generic;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Documents;

namespace CoinstantineAPI.Documents
{
    public class DocumentProvider : IDocumentProvider
    {
        private readonly IDocumentInformationFactory _documentInformationFactory;

        public DocumentProvider(IDocumentInformationFactory documentInformationFactory)
        {
            _documentInformationFactory = documentInformationFactory;
        }

        public async Task<IEnumerable<IDocumentInformation>> GetAllDocuments()
        {
            var whitePaper = await GetInformation(DocumentType.WhitePaper);
            var termsAndServices = await GetInformation(DocumentType.TermsAndServices);
            var privacyPolicy = await GetInformation(DocumentType.PrivacyPolicy);
            return new List<IDocumentInformation>
            {
                whitePaper,
                termsAndServices,
                privacyPolicy
            };
        }

        public Task<IDocumentInformation> GetInformation(DocumentType documentType)
        {
            return _documentInformationFactory.GetDocumentInformation(documentType);
        }
    }
}