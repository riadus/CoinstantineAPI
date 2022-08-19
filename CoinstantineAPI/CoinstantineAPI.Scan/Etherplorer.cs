using System;
using System.Threading.Tasks;
using CoinstantineAPI.Scan.Dtos;
using CoinstantineAPI.Scan.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Scan
{
    public class Etherplorer : IEtherplorer
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger _logger;
        public Etherplorer(IApiClientFactory apiClientFactory,
                           ILoggerFactory loggerFactory)
        {
            _apiClient = apiClientFactory.GetApiClient(ApiType.Etherplorer);
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public Task<EtherplorerResponse> GetInfo(string address)
        {
            try
            {
                return _apiClient.GetAsync<EtherplorerResponse>($"getAddressInfo/{address}");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetInfo()", address);
                throw ex;
            }
        }
    }
}
