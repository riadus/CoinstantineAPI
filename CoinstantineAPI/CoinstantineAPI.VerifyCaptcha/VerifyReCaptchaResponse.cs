using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoinstantineAPI.VerifyCaptcha
{
    public class VerifyReCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("score")]
        public int Score { get; set; }
        [JsonProperty("action")]
        public string Action { get; set; }
        [JsonProperty("challenge_ts")]
        public DateTime? ChallengeTimestamp { get; set; }
        [JsonProperty("hostname")]
        public string Hostname { get; set; }
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
