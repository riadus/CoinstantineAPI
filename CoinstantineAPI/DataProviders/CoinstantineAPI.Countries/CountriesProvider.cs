using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CoinstantineAPI.Core.DataProviders;
using CoinstantineAPI.Data;
using Newtonsoft.Json;

namespace CoinstantineAPI.Countries
{
    public class CountriesProvider : ICountriesProvider
    {
        private readonly Dictionary<char, string> _letters;
        private Dictionary<string, IEnumerable<Country>> _caches;
        public CountriesProvider()
        {
            _caches = new Dictionary<string, IEnumerable<Country>>();
            _letters = new Dictionary<char, string>
            {
                {'A', char.ConvertFromUtf32(0x1F1E6)},
                {'B', char.ConvertFromUtf32(0x1F1E7)},
                {'C', char.ConvertFromUtf32(0x1F1E8)},
                {'D', char.ConvertFromUtf32(0x1F1E9)},
                {'E', char.ConvertFromUtf32(0x1F1EA)},
                {'F', char.ConvertFromUtf32(0x1F1EB)},
                {'G', char.ConvertFromUtf32(0x1F1EC)},
                {'H', char.ConvertFromUtf32(0x1F1ED)},
                {'I', char.ConvertFromUtf32(0x1F1EE)},
                {'J', char.ConvertFromUtf32(0x1F1EF)},
                {'K', char.ConvertFromUtf32(0x1F1F0)},
                {'L', char.ConvertFromUtf32(0x1F1F1)},
                {'M', char.ConvertFromUtf32(0x1F1F2)},
                {'N', char.ConvertFromUtf32(0x1F1F3)},
                {'O', char.ConvertFromUtf32(0x1F1F4)},
                {'P', char.ConvertFromUtf32(0x1F1F5)},
                {'Q', char.ConvertFromUtf32(0x1F1F6)},
                {'R', char.ConvertFromUtf32(0x1F1F7)},
                {'S', char.ConvertFromUtf32(0x1F1F8)},
                {'T', char.ConvertFromUtf32(0x1F1F9)},
                {'U', char.ConvertFromUtf32(0x1F1FA)},
                {'V', char.ConvertFromUtf32(0x1F1FB)},
                {'W', char.ConvertFromUtf32(0x1F1FC)},
                {'X', char.ConvertFromUtf32(0x1F1FD)},
                {'Y', char.ConvertFromUtf32(0x1F1FE)},
                {'Z', char.ConvertFromUtf32(0x1F1FF)}
            };
        }

        public async Task<IEnumerable<Country>> GetCountries(string locale)
        {
            if (!_caches.ContainsKey(locale) || _caches[locale] == null)
            {
                var countries = await ReadFixedJsonResource<Dictionary<string, string>>(locale);
                _caches.Add(locale, countries.OrderBy(x => x.Value).Select(x => new Country { Flag = GetFlag(x.Key), Name = x.Value, Key = x.Key }));
            }
            return _caches[locale];
        }

        public async Task<Country> GetCountryForStore(string key)
        {
            var countries = await GetCountries("fr");
            return countries.FirstOrDefault(x => x.Key == key);
        }

        async Task<T> ReadFixedJsonResource<T>(string locale)
        {
            var assembly = GetType().GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"CoinstantineAPI.Countries.Data.{locale}.country.json");
            string text;
            using (var reader = new StreamReader(stream))
            {
                text = await reader.ReadToEndAsync().ConfigureAwait(false);
            }
            return JsonConvert.DeserializeObject<T>(text);
        }

        private string GetFlag(string country)
        {
            var l1 = country[0];
            var l2 = country[1];
            return _letters[l1] + _letters[l2];
        }
    }
}
