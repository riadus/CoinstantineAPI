using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace CoinstantineAPI.TelegramProvider.Entities
{
    public class AppMessageEntity : MessageEntity
    {
        [JsonProperty(Required = Required.Always)]
        public new string Type { get; set; }
    }
}
