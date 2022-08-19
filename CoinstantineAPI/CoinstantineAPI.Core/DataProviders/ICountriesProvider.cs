using System.Collections.Generic;
using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.DataProviders
{
    public interface ICountriesProvider
    {
        Task<IEnumerable<Country>> GetCountries(string locale);
        Task<Country> GetCountryForStore(string key);
    }
}
