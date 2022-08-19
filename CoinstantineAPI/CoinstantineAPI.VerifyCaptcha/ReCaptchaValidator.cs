using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CoinstantineAPI.Core.Extensions;
using CoinstantineAPI.Core;

namespace CoinstantineAPI.VerifyCaptcha
{
    public class ReCaptchaValidator : IReCaptchaValidator
    {
        private readonly HttpClient _httpClient;
        private readonly ICheckCaptcha _checkCaptcha;

        public ReCaptchaValidator(HttpClient client, ICheckCaptcha checkCaptcha)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
            _checkCaptcha = checkCaptcha;
        }

        public async Task<VerifyReCaptchaResponse> Validate(IHeaderDictionary header)
        {
            header.TryGetValue("client_id", out var clientId);
            header.TryGetValue("secret", out var secret);
            header.TryGetValue("X-CSN-RECAPTCHA", out var reCaptchaResponse);
            if (clientId.ToString().IsNullOrEmpty() || secret.ToString().IsNullOrEmpty())
            {
                return new VerifyReCaptchaResponse()
                {
                    Success = false
                };
            }
            if (!await _checkCaptcha.ShouldCheck(clientId, secret))
            {
                return new VerifyReCaptchaResponse()
                {
                    Success = true
                };
            }

            var parameters = new Dictionary<string, string> { { "secret", Constants.ReCaptchaSecret }, { "response", reCaptchaResponse } };
            var encodedContent = new FormUrlEncodedContent(parameters);

            //Post to google to verify the reCaptcha
            using (HttpResponseMessage response = await _httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", encodedContent))
            {
                // Read and process the response from google to determine if the request may continue or not
                return await response.Content.ReadAsAsync<VerifyReCaptchaResponse>();
            }
        }
    }
}
