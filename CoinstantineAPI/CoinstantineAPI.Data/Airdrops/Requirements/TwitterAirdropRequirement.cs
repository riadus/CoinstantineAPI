using System;

namespace CoinstantineAPI.Data
{
    public class TwitterAirdropRequirement : Entity, IAirdropRequirement
    {
        public bool HasAccount { get; set; }
        public bool HasAccountApplies => HasAccount;
        public int MinimumFollowers { get; set; }
        public bool MinimumFollowersApplies => MinimumFollowers > 0;
        public DateTime? MinimumCreationDate { get; set; }
        public bool MinimumCreationDateApplies => MinimumCreationDate != null;

        public AirdropDefinition AirdropDefinition { get; set; }
    }
}
