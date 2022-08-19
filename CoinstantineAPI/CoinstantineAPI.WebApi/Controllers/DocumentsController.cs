using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.Blockchain.Web3;
using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Documents;
using CoinstantineAPI.Core.Encryption;
using CoinstantineAPI.Core.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/documents")]
    [Authorize]
    public class DocumentsController : BaseController
    {
        private readonly IDocumentProvider _documentProvider;

        public DocumentsController(IDocumentProvider documentProvider,
                                   IUsersService usersService, 
                                   ILoggerFactory loggerFactory,
                                   IUserResolverService userResolverService) : base(usersService, loggerFactory, userResolverService)
        {
            _documentProvider = documentProvider;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{documentType}/file")]
        public async Task<IActionResult> GetPDFDocument(DocumentType documentType)
        {
            var documentInfo = await _documentProvider.GetInformation(documentType);
            if (documentInfo.DocumentAvailable)
            {
                var bytes = await documentInfo.GetBytes();
                return File(bytes, documentInfo.FileType.ApplicationTypeString, documentInfo.Filename);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{documentType}")]
        public async Task<IActionResult> GetInformation(DocumentType documentType)
        {
            return Ok(await _documentProvider.GetInformation(documentType));
        }

        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
            return Ok(await _documentProvider.GetAllDocuments());
        }
    }
}
