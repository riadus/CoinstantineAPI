using System;

namespace CoinstantineAPI.Data
{
    public class AirdropSubscriber : Entity
    {
        public int UserId { get; set; }
        public DateTime SubscribtionDate { get; set; }
        public AidropSubscriptionStatus Status { get; set; }
        public AirdropSubscription AirdropSubscription { get; set; }
    }
}
