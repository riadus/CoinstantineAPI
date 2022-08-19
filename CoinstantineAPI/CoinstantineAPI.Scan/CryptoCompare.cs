using System;
using System.Threading.Tasks;
using CoinstantineAPI.Scan.Dtos;
using CoinstantineAPI.Scan.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Scan
{
    public class CryptoCompare : ICryptoCompare
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger _logger;

        public CryptoCompare(IApiClientFactory apiClientFactory,
                             ILoggerFactory loggerFactory)
        {
            _apiClient = apiClientFactory.GetApiClient(ApiType.CryptoCompare);
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<CryptoCompareResponse> GetInfos(string tokenName)
        {
            try
            {
                return await _apiClient.GetAsync<CryptoCompareResponse>($"price?fsym={tokenName}&tsyms=BTC,USD,EUR,ETH");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetInfos()", tokenName);
                throw ex;
            }
        }

        public async Task<CryptoCompareResponse> GetInfosFromEtherdelta(string tokenName)
        {
            try
            {
                return await _apiClient.GetAsync<CryptoCompareResponse>($"price?fsym={tokenName}&tsyms=ETH&e=etherdelta");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetInfosFromEtherdelta()", tokenName);
                throw ex;
            }
        }

        public async Task<CryptoCompareResponse> GetEtherPrice()
        {
            try
            {
                return await _apiClient.GetAsync<CryptoCompareResponse>($"price?fsym=ETH&tsyms=BTC,USD,EUR");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetEtherPrice()");
                throw ex;
            }
        }
    }
}
