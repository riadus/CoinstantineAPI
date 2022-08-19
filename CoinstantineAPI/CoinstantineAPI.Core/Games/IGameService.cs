using System.Threading.Tasks;

namespace CoinstantineAPI.Core.Games
{
    public interface IGameService
    {
        Task<bool> GiveAway(string game, string userId, int amount, string callerId, string description);
    }
}
