namespace CoinstantineAPI.WebApi.Responses
{
    public class TelegramAirdropRequirementResponse
    {
        public bool HasAccount { get; set; }
        public bool HasAccountApplies => HasAccount;
    }
}
