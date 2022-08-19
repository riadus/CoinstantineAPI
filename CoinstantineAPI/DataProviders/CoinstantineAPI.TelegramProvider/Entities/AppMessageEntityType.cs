using Newtonsoft.Json;

namespace CoinstantineAPI.TelegramProvider.Entities
{
    public enum AppMessageEntityType
    {
        [JsonProperty("mention")]
        Mention,
        [JsonProperty("hashtag")]
        Hashtag,
        [JsonProperty("bot_command")]
        BotCommand,
        [JsonProperty("url")]
        Url,
        [JsonProperty("email")]
        Email,
        [JsonProperty("bold")]
        Bold,
        [JsonProperty("italic")]
        Italic,
        [JsonProperty("code")]
        Code,
        [JsonProperty("pre")]
        Pre,
        [JsonProperty("text_link")]
        TextLink,
        [JsonProperty("text_mention")]
        TextMention,
        [JsonProperty("phone_number")]
        PhoneNumber,
        [JsonProperty("cashtag")]
        Cashtag,
        [JsonProperty()]
        Unknown
    }
}
