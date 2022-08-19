using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Users.Unicity
{
    public class UnicityConstraintsChecker : IUnicityConstraintsChecker
    {
        private readonly IUnicityConstraintsFactory _unicityConstraintsFactory;
        private readonly IContextProvider _contextProvider;
        private readonly ILogger _logger;

        public UnicityConstraintsChecker(IUnicityConstraintsFactory unicityConstraintsFactory,
                                         IContextProvider contextProvider,
                                         ILoggerFactory loggerFactory)
        {
            _unicityConstraintsFactory = unicityConstraintsFactory;
            _contextProvider = contextProvider;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        private UnicityResult BuildUnicityResult(bool AllGood, Dictionary<UniqueKey, bool> Results)
        {
            return new UnicityResult
            {
                AllGood = AllGood,
                FailedConstaints = Results.Where(x => !x.Value).Select(x => x.Key),
            };
        }

        public async Task<UnicityResult> CheckUnicity(ApiUser user, UnicityTopic topic)
        {
            try
            {
                var unicityResults = new Dictionary<UniqueKey, bool>();
                var allGood = true;
                var strategie = _unicityConstraintsFactory.GetStrategie(topic);

                foreach (var constraint in strategie.Constraints)
                {
                    var result = await CheckProfileConstraints(user, constraint.ContraintToCheck, constraint.Navigation);
                    allGood &= result;
                    unicityResults.Add(constraint.UniqueKey, result);
                }

                return BuildUnicityResult(allGood, unicityResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CheckUnicity()", user, topic);
                throw ex;
            }
        }

        private async Task<bool> CheckProfileConstraints(ApiUser user, Func<ApiUser, object> constraintFunction, Expression<Func<ApiUser, IProfileItem>> navigationExpression)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    if (constraintFunction(user) == null) { return true; }
                    if (constraintFunction(user) is string)
                    {
                        if (string.IsNullOrEmpty(constraintFunction(user).ToString()))
                        {
                            return true;
                        }
                    }
                    var contraint = constraintFunction(user);
                    IQueryable<ApiUser> query = context.ApiUsers;
                    if (navigationExpression != null)
                    {
                        query = query.Include(navigationExpression);
                    }
                    var apiUsers = await query.ToListAsync();

                    return !apiUsers.Any(x =>
                                  constraintFunction(x) != null && constraintFunction(x).Equals(constraintFunction(user)));
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in CheckProfileConstraints()", user);
                throw ex;
            }
        }

        public bool CheckDiscordUnicity(DiscordProfile discordProfile)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var exists = context.DiscordProfiles?.Any(x => x.DiscordServerName == discordProfile.DiscordServerName && x.DiscordUserIdentifier == discordProfile.DiscordUserIdentifier);
                    return exists ?? false;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in CheckDiscordUnicity()");
                throw ex;
            }
        }
    }
}
