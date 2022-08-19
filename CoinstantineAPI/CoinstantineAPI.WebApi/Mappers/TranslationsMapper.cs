using System.Collections.Generic;
using System.Linq;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Data;
using CoinstantineAPI.WebApi.DTO;

namespace CoinstantineAPI.WebApi.Mappers
{
    public class TranslationsMapper : IMapper<IEnumerable<Translation>, TranslationsDTO>
    {
        private readonly IMapper<Translation, TranslationDTO> _mapper;

        public TranslationsMapper(IMapper<Translation, TranslationDTO> mapper)
        {
            _mapper = mapper;
        }

        public TranslationsDTO Map(IEnumerable<Translation> source)
        {
            var dtos = source.Select(datum => _mapper.Map(datum));
            var translationsDTO = new TranslationsDTO
            {
                NumberOfTranslations = dtos.Count(),
                SupportedLanguages = dtos.Select(x => x.Language).Distinct().ToList(),
                Translations = new List<LanguageTranslationsDTO>()
            };

            foreach (var language in translationsDTO.SupportedLanguages)
            {
                var languageTranslationDTO = new LanguageTranslationsDTO
                {
                    Language = language,
                    NumberOfTranslations = dtos.Count(x => x.Language == language),
                    LanguageTranslations = dtos.Where(x => x.Language == language).ToList()
                };
                translationsDTO.Translations.Add(languageTranslationDTO);
            }

            return translationsDTO;
        }
    }
}
