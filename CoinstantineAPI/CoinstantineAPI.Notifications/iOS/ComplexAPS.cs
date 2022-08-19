using Newtonsoft.Json;

namespace CoinstantineAPI.Notifications.iOS
{
    public class ComplexAPS : APS
    {
        [JsonProperty("alert")]
        public Alert Alert { get; set; }
    }
}
