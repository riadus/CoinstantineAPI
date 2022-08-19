using Newtonsoft.Json;

namespace CoinstantineAPI.Notifications.iOS
{
    public class SimpleAPS : APS
    {
        [JsonProperty("alert")]
        public string Alert { get; set; }
    }
}
