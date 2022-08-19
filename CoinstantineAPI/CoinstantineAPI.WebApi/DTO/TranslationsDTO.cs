using System.Collections.Generic;

namespace CoinstantineAPI.WebApi.DTO
{
    public class TranslationsDTO
    {
        public int NumberOfTranslations { get; set; }
        public List<string> SupportedLanguages { get; set; }
        public List<LanguageTranslationsDTO> Translations { get; set; }
    }
}
