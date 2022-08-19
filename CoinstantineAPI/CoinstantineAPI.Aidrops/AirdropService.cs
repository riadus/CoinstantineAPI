using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.Aidrops.Requirements.Interfaces;
using CoinstantineAPI.Core.Airdrops;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Extensions;
using CoinstantineAPI.Core.Games;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Aidrops
{
    public class AirdropService : IAirdropService
    {
        private readonly IContextProvider _contextProvider;
        private readonly IBountyProgram _bountyProgram;
        private readonly IRequirementToLambda _requirementToLambda;
        private readonly ILogger _logger;

        public AirdropService(IContextProvider contextProvider,
                              IBountyProgram bountyProgram,
                              IRequirementToLambda requirementToLambda,
                              ILoggerFactory loggerFactory)
        {
            _contextProvider = contextProvider;
            _bountyProgram = bountyProgram;
            _requirementToLambda = requirementToLambda;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<IEnumerable<AirdropSubscription>> GetCurrentAirdrops()
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var subscriptions = await context.AirdropSubscriptions
                                                .Include(a => a.Subscribers).ToListAsync();
                    var aidropDefinitions = await context.AirdropDefinitions
                                                         .Include(a => a.BitcoinTalkAirdropRequirement)
                                                         .Include(a => a.TwitterAirdropRequirement)
                                                         .Include(a => a.TelegramAirdropRequirement)
                                                         .Include(a => a.DiscordAirdropRequirement)
                                                         .ToListAsync();

                    subscriptions.ForeachChangeValue(
                        s => s.AirdropDefinition = aidropDefinitions.FirstOrDefault(a => a.Id == s.AirdropDefinitionId));

                    return subscriptions;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetCurrentAirdrops()");
                throw ex;
            }
        }

        public async Task<UserAirdrops> GetUserAidrops(ApiUser user)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    return await context.UserAirdrops.FirstOrDefaultAsync(x => x.UserId == user.Id);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserAidrops()", user);
                throw ex;
            }
        }

        public async Task<AirdropSubscription> GetAirdropSubscription(int airdropId)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var subscription = await context.AirdropSubscriptions
                                                .Include(a => a.Subscribers)
                                                .FirstOrDefaultAsync(x => x.AirdropDefinitionId == airdropId);
                    subscription.AirdropDefinition = await context.AirdropDefinitions
                            .Include(a => a.BitcoinTalkAirdropRequirement)
                            .Include(a => a.TwitterAirdropRequirement)
                            .Include(a => a.TelegramAirdropRequirement)
                            .Include(a => a.DiscordAirdropRequirement)
                        .FirstOrDefaultAsync(x => x.Id == subscription.AirdropDefinitionId);
                    return subscription;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error in GetAirdropSubscription()", airdropId);
                throw ex;
            }
        }

        public async Task<AirdropSubscriptionResult> SubscribeToAirdrop(ApiUser user, int airdropId)
        {
            try
            {
                AirdropDefinition airdropDefinition = null;
                using (var context = _contextProvider.CoinstantineContext)
                {
                    airdropDefinition = await context.AirdropDefinitions
                                                            .Include(a => a.TwitterAirdropRequirement)
                                                            .Include(a => a.TelegramAirdropRequirement)
                                                            .Include(a => a.BitcoinTalkAirdropRequirement)
                                                            .Include(a => a.DiscordAirdropRequirement)
                                                         .FirstOrDefaultAsync(x => x.Id == airdropId);
                }
                if (airdropDefinition == null || airdropDefinition.AirdropType != AirdropType.Airdrop)
                {
                    return new AirdropSubscriptionResult { FailReason = FailReason.UnknownAirdrop };
                }
                if (airdropDefinition.StartDate > DateTime.Now)
                {
                    return new AirdropSubscriptionResult { FailReason = FailReason.NotStarted };
                }

                if (airdropDefinition.ExpirationDate < DateTime.Now && airdropDefinition.ExpirationDate.Year > 1900)
                {
                    return new AirdropSubscriptionResult { FailReason = FailReason.Expired };
                }

                var airdropRequirements = new List<IAirdropRequirement>
            {
                airdropDefinition.BitcoinTalkAirdropRequirement,
                airdropDefinition.TelegramAirdropRequirement,
                airdropDefinition.TwitterAirdropRequirement,
                airdropDefinition.DiscordAirdropRequirement
            };
                if (_requirementToLambda.MeetsAllRequirement(user, airdropRequirements))
                {
                    var subscriber = new AirdropSubscriber
                    {
                        UserId = user.Id,
                        SubscribtionDate = DateTime.Now,
                        Status = AidropSubscriptionStatus.Subscribed
                    };

                    var subscription = await GetAirdropSubscription(airdropId);
                    if (subscription.Subscribers.Any(x => x.UserId == user.Id))
                    {
                        return new AirdropSubscriptionResult { FailReason = FailReason.AlreadySubscribed };
                    }
                    if (subscription.Subscribers.Count() >= airdropDefinition.MaxLimit && airdropDefinition.MaxLimit > -1)
                    {
                        return new AirdropSubscriptionResult { FailReason = FailReason.Full };
                    }
                    subscription.Subscribers.Add(subscriber);

                    var userAirdrops = await GetUserAidrops(user);
                    if (userAirdrops == null)
                    {
                        userAirdrops = new UserAirdrops
                        {
                            UserId = user.Id,
                            AirdropIds = new List<string>
                        {
                            airdropDefinition.Id.ToString()
                        }
                        };
                    }
                    else
                    {
                        userAirdrops.AirdropIds.Add(airdropDefinition.Id.ToString());
                    }

                    using (var context = _contextProvider.CoinstantineContext)
                    {
                        await context.UserAirdrops.AddOrUpdateAsync(userAirdrops);
                        await context.AirdropSubscriptions.AddOrUpdateAsync(subscription);
                        await context.SaveChangesAsync();
                    }
                    await _bountyProgram.ReferralAward(user, airdropDefinition);
                    return new AirdropSubscriptionResult { UserAirdrops = userAirdrops, FailReason = FailReason.None };
                }
                return new AirdropSubscriptionResult { FailReason = FailReason.RequirementsNotMet };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in SubscribeToAirdrop()", user, airdropId);
                throw ex;
            }
        }

        public async Task<bool> CreateAirdrop(AirdropDefinition airdropDefinition)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    await context.AirdropDefinitions.AddOrUpdateAsync(airdropDefinition);
                    var subscription = new AirdropSubscription
                    {
                        AirdropDefinitionId = airdropDefinition.Id,
                        Subscribers = new List<AirdropSubscriber>()
                    };

                    await context.AirdropSubscriptions.AddAsync(subscription);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error in CreateAirdrop()", airdropDefinition);
                return false;
            }
        }

        public async Task<IEnumerable<Game>> GetCurrentGames()
        {

            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var games = await context.Games
                                                .Include(a => a.AirdropDefinition).ToListAsync();
                    var aidropDefinitions = await context.AirdropDefinitions
                                                         .Include(a => a.BitcoinTalkAirdropRequirement)
                                                         .Include(a => a.TwitterAirdropRequirement)
                                                         .Include(a => a.TelegramAirdropRequirement)
                                                         .Include(a => a.DiscordAirdropRequirement)
                                                         .ToListAsync();
                    return games;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCurrentAirdrops()");
                throw ex;
            }
        }

        public async Task<bool> CreateGame(Game game)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    await context.Games.AddOrUpdateAsync(game);
                    var subscription = new AirdropSubscription
                    {
                        AirdropDefinitionId = game.AirdropDefinition.Id,
                        Subscribers = new List<AirdropSubscriber>()
                    };

                    await context.AirdropSubscriptions.AddAsync(subscription);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in CreateGame()", game);
                return false;
            }
        }

        public async Task<Game> GetGame(int gameId, ApiUser user)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                var game = await context.Games.Include(g => g.Achievements)
                                                .ThenInclude(a => a.ApiUser)
                                            .Include(g => g.Achievements)
                                                .ThenInclude(a => a.UserAchievements)
                                            .Include(g => g.AirdropDefinition)
                                            .FirstOrDefaultAsync(x => x.Id == gameId);

                var userAchievements = game.Achievements.Where(x => x.ApiUser.Id == user.Id);
                return new Game
                {
                    Id = gameId,
                    Achievements = userAchievements?.ToList() ?? new List<Achievements>(),
                    AirdropDefinition = game.AirdropDefinition
                };
            }
        }
    }
}
