using System.Collections.Generic;
using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.Airdrops
{
    public interface IAirdropService
    {
        Task<IEnumerable<AirdropSubscription>> GetCurrentAirdrops();
        Task<IEnumerable<Game>> GetCurrentGames();
        Task<UserAirdrops> GetUserAidrops(ApiUser user);
        Task<AirdropSubscription> GetAirdropSubscription(int airdropId);
        Task<Game> GetGame(int gameId, ApiUser user);
        Task<AirdropSubscriptionResult> SubscribeToAirdrop(ApiUser user, int airdropId);
        Task<bool> CreateAirdrop(AirdropDefinition airdropDefinition);
        Task<bool> CreateGame(Game game);
    }
}
