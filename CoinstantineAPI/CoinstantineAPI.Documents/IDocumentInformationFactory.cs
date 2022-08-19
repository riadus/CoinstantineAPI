using System.Threading.Tasks;
using CoinstantineAPI.Core.Documents;

namespace CoinstantineAPI.Documents
{
    public interface IDocumentInformationFactory
    {
        Task<IDocumentInformation> GetDocumentInformation(DocumentType documentType);
    }
}
