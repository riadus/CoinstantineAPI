using System.Collections.Generic;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Aidrops.Requirements.Interfaces
{
    public interface IDiscordRequirement : IRequirement<DiscordProfile>
    {
        DiscordProfile GetProfileFromList(IEnumerable<DiscordProfile> discordProfiles);
    }
}
