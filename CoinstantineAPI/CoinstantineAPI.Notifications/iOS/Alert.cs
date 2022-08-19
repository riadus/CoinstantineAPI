using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoinstantineAPI.Notifications.iOS
{
    public class Alert
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("title-loc-key")]
        public string TitleLocKey { get; set; }
        [JsonProperty("title-loc-args")]
        public List<string> TitleLocArgs { get; set; }
        [JsonProperty("action-loc-key")]
        public string ActionLocKey { get; set; }
        [JsonProperty("loc-key")]
        public string LocKey { get; set; }
        [JsonProperty("loc-args")]
        public List<string> LocArgs { get; set; }
        [JsonProperty("launch-image")]
        public string LaunchImage { get; set; }
    }
}
