using Newtonsoft.Json;

namespace CoinstantineAPI.Notifications.Android
{
    public class Payload : IPayload
    {
        [JsonProperty("data")]
        public AndroidData Data { get; set; }
    }
}
