using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.Airdrops
{
    public class AirdropSubscriptionResult
    {
        public UserAirdrops UserAirdrops { get; set; }
        public bool Success => FailReason == FailReason.None;
        public FailReason FailReason { get; set; }
    }
}
