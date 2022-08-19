using System.Threading.Tasks;
using CoinstantineAPI.Core.DataProviders;
using Microsoft.AspNetCore.Mvc;

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountriesProvider _countriesProvider;

        public CountriesController(ICountriesProvider countriesProvider)
        {
            _countriesProvider = countriesProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries([FromQuery] string locale)
        {
            return Ok(await _countriesProvider.GetCountries(locale));
        }
    }
}
