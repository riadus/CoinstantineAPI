using System;

namespace CoinstantineAPI.WebApi.Responses
{
    public class AirdropDefinitionResponse
    {
        public int Id { get; set; }
        public string AirdropName { get; set; }
        public string TokenName { get; set; }
        public int Amount { get; set; }
        public int MaxLimit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string OtherInfoToDisplay { get; set; }
        public string AirdropType { get; set; }
        public int ReferralAward { get; set; } 

        public TwitterAirdropRequirementResponse TwitterAirdropRequirement { get; set; }
        public TelegramAirdropRequirementResponse TelegramAirdropRequirement { get; set; }
        public BitcoinTalkAirdropRequirementResponse BitcoinTalkAirdropRequirement { get; set; }
        public DiscrodAirdropRequirementResponse DiscordAirdropRequirement { get; set; }
    }
}
