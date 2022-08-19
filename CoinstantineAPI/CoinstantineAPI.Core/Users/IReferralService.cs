using System.Collections.Generic;
using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.Users
{
    public interface IReferralService
    {
        Task<Referral> GetReferralLink(ApiUser apiUser);
        Task<IEnumerable<ApiUser>> GetReferrals(ApiUser apiUser);
        Task<ApiUser> GetGodFather(ApiUser apiUser);
        Task SetReferral(ApiUser apiUser, string referralCode);
    }
}
