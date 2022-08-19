using System.Collections.Generic;

namespace CoinstantineAPI.WebApi.DTO
{
    public class LanguageTranslationsDTO
    {
        public int NumberOfTranslations { get; set; }
        public string Language { get; set; }
        public List<TranslationDTO> LanguageTranslations { get; set; }
    }
}
