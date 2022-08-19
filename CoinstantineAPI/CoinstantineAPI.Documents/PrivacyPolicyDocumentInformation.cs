using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Documents;
using Microsoft.Extensions.FileProviders;
using Microsoft.WindowsAzure.Storage;

namespace CoinstantineAPI.Documents
{
    public class PrivacyPolicyDocumentInformation : DocumentInformation
    {
        private readonly FileType _fileType;
        public PrivacyPolicyDocumentInformation(CloudStorageAccount cloudStorageAccount, IFileProvider fileProvider, IContextProvider contextProvider) : base(cloudStorageAccount, fileProvider, contextProvider)
        {
            _fileType = new FileType
            {
                ApplicationType = FileApplicationType.Web,
                ApplicationTypeString = "application/html"
            };
        }

        public override string Filename => "https://www.coinstantine.io/legal-notice/privacy-policy/";
        public override DocumentType DocumentType => DocumentType.PrivacyPolicy;
        public override FileType FileType => _fileType;
    }
}
