using System;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.WebApi.Responses
{
    public class AirdropSubscriberResponse
    {
        public int UserId { get; set; }
        public DateTime SubscribtionDate { get; set; }
        public AidropSubscriptionStatus Status { get; set; }
    }
}
