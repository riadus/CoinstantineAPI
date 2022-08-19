using System;

namespace CoinstantineAPI.WebApi.Responses
{
    public class TwitterAirdropRequirementResponse
    {
        public bool HasAccount { get; set; }
        public bool HasAccountApplies => HasAccount;
        public int MinimumFollowers { get; set; }
        public bool MinimumFollowersApplies => MinimumFollowers > 0;
        public DateTime? MinimumCreationDate { get; set; }
        public bool MinimumCreationDateApplies => MinimumCreationDate != null;
    }
}
