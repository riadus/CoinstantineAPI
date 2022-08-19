using CoinstantineAPI.Core.Services;
using Newtonsoft.Json;

namespace CoinstantineAPI.Notifications.Android
{
    public class AndroidData
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("silent")]
        public int Silent { get; set; }
        [JsonProperty("part-to-update")]
        public PartToUpdate? PartToUpdate { get; set; }
        [JsonProperty("translationKey")]
        public string TranslationKey { get; set; }

    }
}
