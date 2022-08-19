using System.Collections.Generic;
using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.Blockchain
{
    public interface ICoinstantineService
    {
        Task<string> CreateAirdrop(Airdrop airdrop);
        Task<bool> Subscribe(string userId, string airdropId);
        Task<bool> Deposit(string airdropId, int amount);
        Task<string> CheckDeposit(string airdropId);
        Task<IEnumerable<Subscriber>> Subscribers(string airdropId);
        Task<bool> Withdraw(string airdropId, string userId);
        Task<bool> CloseAirdrop(string airdropId);
        Task<bool> StartDistribution(string airdropId);
    }
}
