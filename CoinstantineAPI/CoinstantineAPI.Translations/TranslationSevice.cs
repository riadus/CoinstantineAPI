using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Translations
{
    public class TranslationSevice : ITranslationService
	{
        private readonly IContextProvider _contextProvider;
        private readonly ILogger _logger;
        private List<Translation> _cachedTranslations;
        private bool _reloadCache;
        public TranslationSevice(IContextProvider contextProvider,
                                 ILoggerFactory loggerFactory)
		{
			_contextProvider = contextProvider;
            _logger = loggerFactory.CreateLogger(GetType());
		}

		public async Task<IEnumerable<Translation>> GetAllTranslations()
		{
            if((_cachedTranslations?.Any() ?? false) && !_reloadCache)
            {
                return _cachedTranslations;
            }
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    _reloadCache = false;
                    return _cachedTranslations = await context.Translations.ToListAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllTranslations()");
                throw ex;
            }
		}

		public async Task<IEnumerable<Translation>> GetTranslations(string language)
		{
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    return await context.Translations
                                    .Where(x => x.Language == language)
                                    .ToListAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "GetTranslations()", language);
                throw ex;
            }
		}

		public async Task SaveTranslation(Translation translation, string language)
		{
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var entity = await context.Translations.FirstOrDefaultAsync(x => x.Key == translation.Key && translation.Language == language);
                    if (entity != null)
                    {
                        context.Translations.Remove(entity);
                    }
                    translation.Language = language;
                    await context.Translations.AddAsync(translation);
                    await context.SaveChangesAsync();
                }
                _reloadCache = true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in SaveTranslation()", translation, language);
                throw ex;
            }
		}

		public async Task SaveTranslations(IEnumerable<Translation> translations, string language)
		{
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var currentTranslations = context.Translations.Where(x => x.Language == language);
                    context.Translations.RemoveRange(currentTranslations);

                    foreach (var translation in translations)
                    {
                        translation.Language = language;
                    }
                    await context.Translations.AddRangeAsync(translations);
                    await context.SaveChangesAsync();
                }
                _reloadCache = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveTranslations()", language);
                throw ex;
            }
		}
	}
}
