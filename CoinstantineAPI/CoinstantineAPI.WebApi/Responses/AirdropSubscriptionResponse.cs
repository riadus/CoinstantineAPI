using System.Collections.Generic;

namespace CoinstantineAPI.WebApi.Responses
{
    public class AirdropSubscriptionResponse
    {
        public AirdropDefinitionResponse AirdropDefinition { get; set; }
        public ICollection<AirdropSubscriberResponse> Subscribers { get; set; }
        public int Count => Subscribers.Count;
    }
}
