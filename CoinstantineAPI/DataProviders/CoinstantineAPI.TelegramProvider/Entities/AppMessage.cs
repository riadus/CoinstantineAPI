using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace CoinstantineAPI.TelegramProvider.Entities
{
    public class AppMessage : Message
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public new AppMessageEntity[] Entities { get; set; }
    }
}
