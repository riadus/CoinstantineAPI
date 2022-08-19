using System;

namespace CoinstantineAPI.Data
{
    public class DiscordProfile : Entity, IProfileItem
    {
        public string Username { get; set; }
        public string DiscordUserIdentifier { get; set; }
        public bool Validated { get; set; }
        public DateTime? ValidationDate { get; set; }
        public DateTime? JoinedDate { get; set; }
        public string DiscordChannelUrl { get; set; }
        public string DiscordServerName { get; set; } 
        public ApiUser ApiUser { get; set; }
    }
}
