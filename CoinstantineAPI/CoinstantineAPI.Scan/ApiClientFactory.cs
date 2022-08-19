using System.Collections.Generic;
using CoinstantineAPI.Scan.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Scan
{
    public class ApiClientFactory : IApiClientFactory
    {
        private readonly Dictionary<ApiType, ApiInfo> _dictionary;
        private readonly ILoggerFactory _loggerFactory;

        public ApiClientFactory(ILoggerFactory loggerFactory)
        {
            _dictionary = new Dictionary<ApiType, ApiInfo>()
            {
                { ApiType.Etherplorer, new ApiInfo { BaseAddress = "https://api.ethplorer.io/", Token = "freekey"}},
                { ApiType.CryptoCompare, new ApiInfo { BaseAddress = "https://min-api.cryptocompare.com/data/", Token = ""}},
            };
            _loggerFactory = loggerFactory;
        }

        public IApiClient GetApiClient(ApiType type)
        {
            return new ApiClient(_dictionary[type], _loggerFactory);
        }
    }
}
