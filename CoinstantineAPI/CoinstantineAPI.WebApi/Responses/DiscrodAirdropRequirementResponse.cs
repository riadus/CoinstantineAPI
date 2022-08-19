namespace CoinstantineAPI.WebApi.Responses
{
    public class DiscrodAirdropRequirementResponse
    {
        public bool NeedsToJoinServer { get; set; }
        public bool NeedsToJoinServerApplies { get; set; }
        public string ServerUrl { get; set; }
        public string ServerName { get; set; }
    }
}
