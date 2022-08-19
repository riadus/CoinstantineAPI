using System.Collections.Generic;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using CoinstantineAPI.WebApi.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/translations")]
    [Authorize]
    public class TranslationsController : BaseController
    {
        private readonly ITranslationService _translationService;
        private readonly IMapper<IEnumerable<Translation>, TranslationsDTO> _translationsMapper;

        public TranslationsController(ITranslationService translationService,
                                      IMapper<IEnumerable<Translation>, TranslationsDTO> translationsMapper,
                                      IUsersService usersService,
                                      ILoggerFactory loggerFactory,
                                      IUserResolverService userResolverService) : base(usersService, loggerFactory, userResolverService)
        {
            _translationService = translationService;
            _translationsMapper = translationsMapper;
        }
        // GET api/csvtest
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting all translations");
            var data = await _translationService.GetAllTranslations();
            var translationsDTO = _translationsMapper.Map(data);
            return Ok(translationsDTO);
        }

        [HttpGet]
        [Route("{language}")]
        public async Task<IActionResult> GetForLanguage([FromRoute]string language)
        {
            _logger.LogInformation("Getting translations for specific language", language);
            var data = await _translationService.GetTranslations(language);
            return Ok(_translationsMapper.Map(data));
        }


        [HttpGet]
        [Route("data.csv")]
        [Produces("text/csv")]
        public Task<IActionResult> GetDataAsCsv()
        {
            _logger.LogInformation("Getting translations as CSV");
            return Get();
        }

        [HttpGet]
        [Route("{language}/data.csv")]
        [Produces("text/csv")]
        public Task<IActionResult> GetDataAsCsvForLanguage([FromRoute]string language)
        {
            _logger.LogInformation("Getting translations as CSV for specific language", language);
            return GetForLanguage(language);
        }

        // POST api/csvtest/import
        [HttpPost]
        [Route("{language}")]
        public async Task<IActionResult> ImportTranslations([FromBody]List<Translation> translations, [FromRoute]string language)
        {
            _logger.LogInformation("Importing translations for specific language", language);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                await _translationService.SaveTranslations(translations, language);
                return Ok();
            }
        }

        [HttpPost]
        [Route("{language}/{key}")]
        public async Task<IActionResult> ImportTranslation([FromBody]Translation translation, [FromRoute]string language)
        {
            _logger.LogInformation("Importing translations for specific language and specific key", language);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                await _translationService.SaveTranslation(translation, language);
                return Ok();
            }
        }

        [HttpPost]
        [Route("fixed")]
        public IActionResult FixedTranslations([FromBody]List<Translation> translations, [FromRoute]string language)
        {
            _logger.LogInformation("Posting and getting fixed translations");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var translationsDTO = _translationsMapper.Map(translations);
                return Ok(translationsDTO);
            }
        }
    }
}