namespace CoinstantineAPI.Data
{
    public class DiscordAirdropRequirement : Entity, IAirdropRequirement
    {
        public bool NeedsToJoinServer { get; set; }
        public bool NeedsToJoinServerApplies => NeedsToJoinServer;
        public string ServerUrl { get; set; }
        public string ServerName { get; set; }

        public AirdropDefinition AirdropDefinition { get; set; }
    }
}
