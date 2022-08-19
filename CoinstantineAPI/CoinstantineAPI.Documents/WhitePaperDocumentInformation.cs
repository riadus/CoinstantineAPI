using System;
using System.IO;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Documents;
using Microsoft.Extensions.FileProviders;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;

namespace CoinstantineAPI.Documents
{
    public class WhitePaperDocumentInformation : DocumentInformation
    {
        private readonly FileType _fileType;

        public WhitePaperDocumentInformation(CloudStorageAccount cloudStorageAccount, IFileProvider fileProvider, IContextProvider contextProvider) : base(cloudStorageAccount, fileProvider, contextProvider)
        {
            _fileType = new FileType
            {
                ApplicationType = FileApplicationType.PDF,
                ApplicationTypeString = "application/pdf"
            };
        }

        public override string Filename => "WhitePaper.pdf";
        public override DocumentType DocumentType => DocumentType.WhitePaper;
        public override FileType FileType => _fileType;
    }
}