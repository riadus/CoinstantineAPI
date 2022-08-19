using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoinstantineAPI.Users.Referals
{
    public class ReferralService : IReferralService
    {
        private readonly IContextProvider _contextProvider;
        private readonly ICodeGenerator _codeGenerator;

        public ReferralService(IContextProvider contextProvider, ICodeGenerator codeGenerator)
        {
            _contextProvider = contextProvider;
            _codeGenerator = codeGenerator;
        }

        public async Task<Referral> GetReferralLink(ApiUser apiUser)
        {
            var firstGeneration = false;
            using (var context = _contextProvider.CoinstantineContext)
            {
                var referral = await context.Referrals
                                             .Include(x => x.GodFather)
                                             .FirstOrDefaultAsync(x => x.GodFather.Id == apiUser.Id);
                var loadedApiUser = await context.ApiUsers.FirstOrDefaultAsync(x => x.Id == apiUser.Id);
                if (referral == null)
                {
                    referral = GenerateReferral(loadedApiUser);
                    context.Referrals.Add(referral);
                    await context.SaveChangesAsync();
                    firstGeneration = true;
                }
                referral.FirstGeneration = firstGeneration;
                return referral;
            }
        }

        private Referral GenerateReferral(ApiUser apiUser)
        {
            var code = _codeGenerator.GenerateCode(8);
            return new Referral
            {
                CreationDateTime = DateTime.Now,
                Code = code,
                Users = new List<ApiUser>(),
                GodFather = apiUser
            };
        }

        public async Task<IEnumerable<ApiUser>> GetReferrals(ApiUser apiUser)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                var referral = await context.Referrals
                                            .Include(x => x.GodFather)
                                            .Include(x => x.Users)
                                                .ThenInclude(x => x.UserIdentity)
                                            .FirstOrDefaultAsync(x => x.GodFather.Id == apiUser.Id);
                return referral?.Users ?? new List<ApiUser>();
            }
        }


        public async Task SetReferral(ApiUser apiUser, string referralCode)
        {
            if(string.IsNullOrEmpty(referralCode))
            {
                return;
            }
            using (var context = _contextProvider.CoinstantineContext)
            {
                var referral = await context.Referrals
                                               .Include(x => x.Users)
                                           .FirstOrDefaultAsync(x => x.Code == referralCode);

                if (referral == null)
                {
                    return;
                }
                var loadedApiUser = await context.ApiUsers.FirstOrDefaultAsync(x => x.Id == apiUser.Id);
                referral.Users.Add(loadedApiUser);
                context.Referrals.Update(referral);

                await context.SaveChangesAsync();
            }
        }

        public async Task<ApiUser> GetGodFather(ApiUser apiUser)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                var referral = await context.Referrals
                                             .Include(x => x.Users)
                                             .FirstOrDefaultAsync(x => x.Users.Contains(apiUser));
                return referral?.GodFather;
            }
        }
    }
}
