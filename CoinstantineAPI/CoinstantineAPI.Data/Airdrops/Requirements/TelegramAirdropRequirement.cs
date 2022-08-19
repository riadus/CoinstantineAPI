namespace CoinstantineAPI.Data
{
    public class TelegramAirdropRequirement : Entity, IAirdropRequirement
    {
        public bool HasAccount { get; set; }
        public bool HasAccountApplies => HasAccount;

        public AirdropDefinition AirdropDefinition { get; set; }
    }
}
