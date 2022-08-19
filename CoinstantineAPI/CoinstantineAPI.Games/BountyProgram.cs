using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Games;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Games
{
    public class BountyProgram : IBountyProgram
    {
        private readonly IContextProvider _contextProvider;
        private readonly ILogger<BountyProgram> _logger;

        public BountyProgram(IContextProvider contextProvider, ILogger<BountyProgram> logger)
        {
            _contextProvider = contextProvider;
            _logger = logger;
        }

        public async Task<bool> ReferralAward(ApiUser sponsored, AirdropDefinition airdropDefinition)
        {
            try
            {
                ApiUser godFather = null;
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var referral = await context.Referrals
                                            .Include(x => x.GodFather)
                                                .ThenInclude(x => x.BlockchainInfo)
                                        .FirstOrDefaultAsync(x => x.Users.Contains(sponsored));
                    godFather = referral?.GodFather;
                }
                if (godFather == null)
                {
                    return false;
                }
                var amount = airdropDefinition.ReferralAward;
                var referralProgram = await GetBountyProgram("Coinstantine Referral");

                var achievements = referralProgram.Achievements.FirstOrDefault(x => x.ApiUser.Id == godFather.Id);
                if (achievements == null)
                {
                    achievements = new Achievements
                    {
                        ApiUser = godFather,
                        UserAchievements = new List<UserAchievement>()
                    };
                    referralProgram.Achievements.Add(achievements);
                }
                achievements.UserAchievements.Add(new UserAchievement
                {
                    Achieved = true,
                    PercentageDone = 100,
                    AchievementName = "Referral program",
                    AchievementDate = DateTime.Now,
                    Description = $"{sponsored.Username} subscribed to {airdropDefinition.AirdropName}",
                    Value = amount,
                    Giver = "Coinstantine",
                    Source = "API"
                });

                godFather.BlockchainInfo.Coinstantine += amount;
                using (var context = _contextProvider.CoinstantineContext)
                {
                    context.Achievements.Update(achievements);
                    context.Games.Update(referralProgram);
                    await context.SaveChangesAsync();
                }

                using (var context = _contextProvider.CoinstantineContext)
                {
                    context.BlockchainInfos.Update(godFather.BlockchainInfo);
                    await context.SaveChangesAsync();
                }
                return true;
            }
            catch(Exception e)
            {
                _logger.LogError("Error while rewarding", e);
                return false;
            }
        }

        private async Task<Game> GetBountyProgram(string bountyProgram)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                return await context.Games
                    .Include(x => x.AirdropDefinition)
                    .Include(x => x.Achievements)
                        .ThenInclude(x => x.ApiUser)
                    .Include(x => x.Achievements)
                        .ThenInclude(x => x.UserAchievements)
                    .FirstOrDefaultAsync(x => x.AirdropDefinition.AirdropName == bountyProgram && x.AirdropDefinition.AirdropType == AirdropType.BountyProgram);
            }
        }
    }
}
