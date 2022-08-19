using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CoinstantineAPI.Scan.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Scan
{
    public class ApiClient : IApiClient
    {
        private readonly string _token;
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public ApiClient(ApiInfo apiInfo, 
                         ILoggerFactory loggerFactory)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(apiInfo.BaseAddress)
            };
            _token = apiInfo.Token;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                var urlWithToken = ApplyToken(url);
                var response = await GetContent(urlWithToken).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var deserializedObject = JsonConvert.DeserializeObject<T>(content);
                return deserializedObject;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetAsync<T>()", typeof(T), url);
                throw ex;
            }

        }

        private async Task<HttpResponseMessage> GetContent(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            try
            {
                Debug.WriteLine($"Calling {url}");
                _logger.LogDebug($"Calling {url}");
                var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Request succeeded");
                    _logger.LogDebug($"Request succeeded");
                    return response;
                }
                Debug.WriteLine("Request failed");
                _logger.LogDebug($"Request failed");
                throw new Exception("Something wrong happened with the API");
            }
            catch (TaskCanceledException tce)
            {
                Debug.WriteLine($"request {url} timed out");
                _logger.LogError(tce, "request timed out", url);
                throw new HttpRequestException("Request timed out", tce);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HttpClient Get request {url} exception {ex}");
                _logger.LogError(ex, "Error in GetContent()", url);
                throw;
            }
        }

        private string ApplyToken(string url)
        {
            if (string.IsNullOrEmpty(_token))
            {
                return url;
            }

            url += $"?apiKey={_token}";
            return url;
        }
    }
}
