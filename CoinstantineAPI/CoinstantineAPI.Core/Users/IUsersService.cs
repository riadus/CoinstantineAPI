using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.Users
{
    public interface IUsersService
    {
        Task<bool> IsUsernameUsed(string username);
        Task<bool> ApiUserExists(string username);
        Task<bool> IsEmailUsed(string email);

        Task<UnicityResult> SetBctProfile(ApiUser user);
        Task<UnicityResult> SetTwitterProfile(ApiUser user);
        Task<UnicityResult> SetTelegramProfile(ApiUser user);
        Task<UnicityResult> SaveProfile(ApiUser user);

        Task<bool> RemoveBctProfile(ApiUser user);
        Task<bool> RemoveTwitterProfile(ApiUser user);
        Task<bool> RemoveTelegramProfile(ApiUser user);

        Task UpdateBctProfile(BitcoinTalkProfile bitcoinTalkProfile);
        Task UpdateTwitterProfile(TwitterProfile twitterProfile);

        Task<ApiUser> GetUserFromEmail(string email);
        Task<ApiUser> GetUserFromUsername(string username);
        Task<UserIdentity> GetUserFromUserId(string userId);
        Task<UserIdentity> GetUserIdentityFromEmail(string email);
        Task<BlockchainUser> GetBlockchainUserFromEmail(string email);

        Task<bool> UserExists(string username);

        Task<bool> AddDiscordProfile(ApiUser user, DiscordProfile discordProfile);
        Task<ApiUser> GetUserFromDiscord(string callerId, string serverName);
    }
}
