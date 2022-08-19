using System;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace CoinstantineAPI.TelegramProvider.Entities
{
    public class AppUpdate : Update
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public new AppMessage ChannelPost { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public new AppMessage EditedChannelPost { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public new AppMessage Message { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public new AppMessage EditedMessage { get; set; }
    }
}
