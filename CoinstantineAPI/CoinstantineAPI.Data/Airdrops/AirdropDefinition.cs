using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoinstantineAPI.Data
{
    public class AirdropDefinition : Entity
    {
        public string AirdropName { get; set; }
        public string TokenName { get; set; }
        public int Amount { get; set; }
        public int MaxLimit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string OtherInfoToDisplay { get; set; }
        public AirdropType AirdropType { get; set; }
        public int ReferralAward { get; set; }

        [ForeignKey("TwitterAirdropRequirementId")]
        public TwitterAirdropRequirement TwitterAirdropRequirement { get; set; }
        [ForeignKey("TelegramAirdropRequirementId")]
        public TelegramAirdropRequirement TelegramAirdropRequirement { get; set; }
        [ForeignKey("BitcoinTalkAirdropRequirementId")]
        public BitcoinTalkAirdropRequirement BitcoinTalkAirdropRequirement { get; set; }
        [ForeignKey("DiscordAirdropRequirementId")]
        public DiscordAirdropRequirement DiscordAirdropRequirement { get; set; }

        public AirdropSubscription AirdropSubscription { get; set; }
    }

    public enum AirdropType
    {
        Airdrop = 0,
        BountyProgram = 1,
        Game = 2
    }

    public class Game : Entity
    {
        public AirdropDefinition AirdropDefinition { get; set; }
        public List<Achievements> Achievements { get; set; }
    }

    public class UserAchievement : Entity
    {
        public string AchievementName { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public DateTime AchievementDate { get; set; }
        public bool Achieved { get; set; }
        public int PercentageDone { get; set; }
        public string Giver { get; set; }
        public string Source { get; set; }
    }

    public class Achievements : Entity
    {
        public ApiUser ApiUser { get; set; }
        public List<UserAchievement> UserAchievements { get; set; }
    }
}
