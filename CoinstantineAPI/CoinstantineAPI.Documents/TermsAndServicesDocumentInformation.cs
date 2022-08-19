using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Documents;
using Microsoft.Extensions.FileProviders;
using Microsoft.WindowsAzure.Storage;

namespace CoinstantineAPI.Documents
{
    public class TermsAndServicesDocumentInformation : DocumentInformation
    {
        private readonly FileType _fileType;
        public TermsAndServicesDocumentInformation(CloudStorageAccount cloudStorageAccount, IFileProvider fileProvider, IContextProvider contextProvider) : base(cloudStorageAccount, fileProvider, contextProvider)
        {
            _fileType = new FileType
            {
                ApplicationType = FileApplicationType.PDF,
                ApplicationTypeString = "application/pdf"
            };
        }

        public override string Filename => "TermsAndServices.pdf";
        public override DocumentType DocumentType => DocumentType.TermsAndServices;
        public override FileType FileType => _fileType;
    }
}
