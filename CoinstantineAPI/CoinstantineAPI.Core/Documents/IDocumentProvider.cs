using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoinstantineAPI.Core.Documents
{
    public interface IDocumentProvider
    {
        Task<IEnumerable<IDocumentInformation>> GetAllDocuments();
        Task<IDocumentInformation> GetInformation(DocumentType documentType);
    }
}
