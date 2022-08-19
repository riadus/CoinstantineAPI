using System;
using System.Threading.Tasks;

namespace CoinstantineAPI.Core.Documents
{
    public interface IDocumentInformation
    {
        string Filename { get; }
        DocumentType DocumentType { get; }
        bool DocumentAvailable { get; }
        DateTime? LastModifiedDate { get; }
        FileType FileType { get; }
        Task<byte[]> GetBytes();
        Task Init();
    }
}
