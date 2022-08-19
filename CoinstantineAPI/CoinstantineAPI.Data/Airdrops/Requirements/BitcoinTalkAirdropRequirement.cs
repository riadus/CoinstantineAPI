using System;

namespace CoinstantineAPI.Data
{
    public class BitcoinTalkAirdropRequirement : Entity, IAirdropRequirement
    {
        public bool HasAccount { get; set; }
        public bool HasAccountApplies => HasAccount;
        public int MinimumPosts { get; set; }
        public bool MinimumPostsApplies => MinimumPosts > 0;
        public int MinimumActivity { get; set; }
        public bool MinimumActivityApplies => MinimumActivity > 0;
        public BitcoinTalkRank? MinimumRank { get; set; }
        public bool MinimumRankApplies => MinimumRank != null;
        public BitcoinTalkRank? ExactRank { get; set; }
        public bool ExactRankApplies => ExactRank != null && !MinimumRankApplies;
        public DateTime? MinimumCreationDate { get; set; }
        public bool MinimumCreationDateApplies => MinimumCreationDate != null;

        public AirdropDefinition AirdropDefinition { get; set; }
    }
}
