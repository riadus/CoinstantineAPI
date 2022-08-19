using System.Threading.Tasks;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Users.Unicity
{
    public interface IUnicityConstraintsChecker
    {
        Task<UnicityResult> CheckUnicity(ApiUser user, UnicityTopic topic);
        bool CheckDiscordUnicity(DiscordProfile discordProfile);
    }
}
