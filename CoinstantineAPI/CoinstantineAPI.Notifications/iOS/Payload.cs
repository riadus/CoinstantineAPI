using Newtonsoft.Json;

namespace CoinstantineAPI.Notifications.iOS
{
    public class Payload : IPayload
    {
        [JsonProperty("aps")]
        public APS APS { get; set; }
    }
}
