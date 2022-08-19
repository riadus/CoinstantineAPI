using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.Services
{
    public interface ITranslationService
    {
		Task SaveTranslation(Translation translation, string language);
		Task SaveTranslations(IEnumerable<Translation> translation, string language);
		Task<IEnumerable<Translation>> GetTranslations(string language);
		Task<IEnumerable<Translation>> GetAllTranslations();
    }
}
