using System;
using System.IO;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Documents;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;

namespace CoinstantineAPI.Documents
{
    public abstract class DocumentInformation : IDocumentInformation
    {
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly IFileProvider _fileProvider;
        private readonly IContextProvider _contextProvider;

        protected DocumentInformation(CloudStorageAccount cloudStorageAccount, IFileProvider fileProvider, IContextProvider contextProvider)
        {
            _cloudStorageAccount = cloudStorageAccount;
            _fileProvider = fileProvider;
            _contextProvider = contextProvider;
        }

        public abstract string Filename { get; }
        public abstract DocumentType DocumentType { get; }
        public bool DocumentAvailable { get; private set; }
        public DateTime? LastModifiedDate { get; private set; }
        public virtual FileType FileType { get; }

        public virtual async Task DownloadFile()
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                var document = await context.Documents.FirstOrDefaultAsync(x => x.Filename == Filename);
                if (!document?.DocumentAvailableOnline ?? true)
                {
                    return;
                }
            }
            CloudFileClient fileClient = _cloudStorageAccount.CreateCloudFileClient();

            // Get a reference to the file share we created previously.
            CloudFileShare share = fileClient.GetShareReference("documents");

            // Ensure that the share exists.
            if (await share.ExistsAsync())
            {
                // Get a reference to the root directory for the share.
                CloudFileDirectory rootDir = share.GetRootDirectoryReference();

                // Get a reference to the directory we created previously.
                //CloudFileDirectory sampleDir = rootDir.GetDirectoryReference("documents");

                // Ensure that the directory exists.
                if (await rootDir.ExistsAsync())
                {
                    using (var context = _contextProvider.CoinstantineContext)
                    {
                        var document = await context.Documents.FirstOrDefaultAsync(x => x.Filename == Filename);

                        // Get a reference to the file we created previously.
                        CloudFile file = rootDir.GetFileReference(document.AzureFilename);
                        // Ensure that the file exists.
                        if (await file.ExistsAsync())
                        {
                            var extension = Filename.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase) ? string.Empty : ".pdf";
                            Directory.CreateDirectory("Documents");
                            await file.DownloadToFileAsync($"Documents/{Filename}{extension}", FileMode.Create);
                            DocumentAvailable = true;
                            LastModifiedDate = file.Properties?.LastModified?.DateTime;
                            var version = 1;
                            if (document.ModifiedDate == LastModifiedDate)
                            {
                                document.LastCheck = DateTime.Now;
                                context.Documents.Update(document);
                                await context.SaveChangesAsync();
                                return;
                            }
                            version += document.Version;
                            context.Documents.Remove(document);
                            var newDocument = new Document
                            {
                                Filename = Filename,
                                LastCheck = DateTime.Now,
                                DownloadDate = DateTime.Now,
                                ModifiedDate = LastModifiedDate,
                                AzureFilename = document.AzureFilename,
                                DocumentAvailableOnline = document.DocumentAvailableOnline,
                                Version = version,
                                Path = $"Documents/{Filename}{extension}"
                            };
                            context.Documents.Add(newDocument);
                            await context.SaveChangesAsync();
                        }
                        return;
                    }
                }
            }
            DocumentAvailable = false;
        }

        public virtual async Task<byte[]> GetBytes()
        {
            byte[] bytes = null;
            if (FileType?.ApplicationType == FileApplicationType.PDF)
            {
                var file = GetFileInfo();

                using (var stream = file.CreateReadStream())
                {
                    if (stream == null)
                    {
                        return null;
                    }
                    bytes = new byte[stream.Length];
                    await stream.ReadAsync(bytes, 0, bytes.Length);
                }
            }
            return bytes;
        }

        public async Task Init()
        {
            if(FileType?.ApplicationType == FileApplicationType.PDF)
            {
                var fileInfo = GetFileInfo();
                if(fileInfo != null)
                {
                    using (var context = _contextProvider.CoinstantineContext)
                    {
                        var savedDocument = await context.Documents.FirstOrDefaultAsync(x => x.Filename == Filename);
                        if (savedDocument != null)
                        {
                            if (savedDocument.LastCheck.AddSeconds(20) >= DateTime.Now)
                            {
                                DocumentAvailable = true;
                                LastModifiedDate = savedDocument.ModifiedDate;
                                return;
                            }
                        }
                    }
                }
                await CreateDocumentInDatabase();
                await DownloadFile();
            }
        }

        private async Task CreateDocumentInDatabase()
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                if(await context.Documents.AnyAsync(x => x.Filename == Filename))
                {
                    return;
                }
                var extension = Filename.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase) ? string.Empty : ".pdf";
                var newDocument = new Document
                {
                    Filename = Filename,
                    Version = 0,
                    Path = $"Documents/{Filename}{extension}",
                    DocumentAvailableOnline = false
                };
                context.Documents.Add(newDocument);
                await context.SaveChangesAsync();
            }
        }

        protected IFileInfo GetFileInfo()
        {
            var extension = Filename.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase) ? string.Empty : ".pdf";
            return _fileProvider.GetFileInfo($@"Documents/{Filename}{extension}");
        }

        protected DateTime? GetLastModifiedDate()
        {
            var file = GetFileInfo();
            return file?.LastModified.DateTime > new DateTime(2000, 1, 1) ? file?.LastModified.DateTime : null;
        }
    }
}
