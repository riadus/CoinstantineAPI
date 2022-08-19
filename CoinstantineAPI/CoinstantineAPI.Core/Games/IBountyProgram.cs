using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.Games
{
    public interface IBountyProgram
    {
        Task<bool> ReferralAward(ApiUser sponsored, AirdropDefinition airdropDefinition);
    }
}
