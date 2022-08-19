using System.Collections.Generic;
using System.Linq;
using CoinstantineAPI.Aidrops.Requirements.Interfaces;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Aidrops.Requirements
{
    public class DiscordRequirement : Requirement<DiscordProfile>, IDiscordRequirement
    {
        public string DiscordServerName { get; set; }
        public DiscordProfile GetProfileFromList(IEnumerable<DiscordProfile> discordProfiles)
        {
            return discordProfiles?.FirstOrDefault(x => x.DiscordServerName == DiscordServerName);
        }
    }
}
