using CoinstantineAPI.Core.Services;
using Newtonsoft.Json;

namespace CoinstantineAPI.Notifications.iOS
{
	public abstract class APS
    {
		[JsonProperty("badge")]
		public int Badge { get; set; }
		[JsonProperty("sound")]
		public string Sound { get; set; }
		[JsonProperty("content-available")]
		public int? ContentAvailable { get; set; }
		[JsonProperty("category")]
		public string Category { get; set; }
		[JsonProperty("thread-id")]
		public string ThreadId { get; set; }
		[JsonProperty("part-to-update")]
		public PartToUpdate? PartToUpdate { get; set; }
        [JsonProperty("translationKey")]
        public string TranslationKey { get; set; }
    }
}
