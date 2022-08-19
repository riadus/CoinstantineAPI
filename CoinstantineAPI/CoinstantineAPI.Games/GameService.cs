using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Games;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CoinstantineAPI.Games
{
    public class GameService : IGameService
    {
        private readonly IContextProvider _contextProvider;
        private readonly IUsersService _userService;

        public GameService(IContextProvider contextProvider, IUsersService userService)
        {
            _contextProvider = contextProvider;
            _userService = userService;
        }

        public async Task<bool> GiveAway(string game, string userId, int amount, string callerId, string description)
        {
            var caller = await _userService.GetUserFromDiscord(callerId, "Coinstantine");
            if(caller.Username != "Admin")
            {
                throw new Exception("Not authorized");
            }
            var recipient = await _userService.GetUserFromDiscord(userId, "Coinstantine");
            if(recipient == null)
            {
                return false;
            }

            var coinstantineGame = await GetGame(game);

            var achievements = coinstantineGame.Achievements.FirstOrDefault(x => x.ApiUser.Id == recipient.Id);
            if(achievements == null)
            {
                achievements = new Achievements
                {
                    ApiUser = recipient,
                    UserAchievements = new List<UserAchievement>()
                };
                coinstantineGame.Achievements.Add(achievements);
            }
            achievements.UserAchievements.Add(new UserAchievement
            {
                Achieved = true,
                PercentageDone = 100,
                AchievementName = "GiveAway",
                AchievementDate = DateTime.Now,
                Description = description,
                Value = amount,
                Giver = caller.Username,
                Source = "Discord"
            });

            recipient.BlockchainInfo.Coinstantine += amount;
            using (var context = _contextProvider.CoinstantineContext)
            {
                context.Achievements.Update(achievements);
                context.Games.Update(coinstantineGame);
                await context.SaveChangesAsync();
            }

            using (var context = _contextProvider.CoinstantineContext)
            {
                context.BlockchainInfos.Update(recipient.BlockchainInfo);
                await context.SaveChangesAsync();
            }
            return true;
        }

        private async Task<Game> GetGame(string game)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                return await context.Games
                    .Include(x => x.AirdropDefinition)
                    .Include(x => x.Achievements)
                        .ThenInclude(x => x.ApiUser)
                    .Include(x => x.Achievements)
                        .ThenInclude(x => x.UserAchievements)
                    .FirstOrDefaultAsync(x => x.AirdropDefinition.AirdropName == game);
            }
        }
    }
}
