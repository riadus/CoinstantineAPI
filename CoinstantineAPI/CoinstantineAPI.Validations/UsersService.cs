using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Extensions;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using CoinstantineAPI.Users.Unicity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Users
{
    public class UsersService : IUsersService
    {
        private readonly IContextProvider _contextProvider;
        private readonly INotificationCenter _notificationCenter;
        private readonly IUnicityConstraintsChecker _unicityConstraintsChecker;
        private readonly ILogger _logger;

        public UsersService(IContextProvider contextProvider, 
                            INotificationCenter notificationCenter, 
                            IUnicityConstraintsChecker unicityConstraintsChecker,
                            ILoggerFactory loggerFactory)
        {
            _contextProvider = contextProvider;
            _notificationCenter = notificationCenter;
            _unicityConstraintsChecker = unicityConstraintsChecker;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<bool> IsUsernameUsed(string username)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    return await context.UserIdentities.AnyAsync(a => a.Username.ToLower() == username.ToLower());
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in IsUsernameUsed()", username);
                throw ex;
            }
        }

        public async Task<UserIdentity> GetUserIdentityFromEmail(string email)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    return await context.UserIdentities
                                            .Include(x => x.RefreshTokens)
                                                .ThenInclude(x => x.Application)
                                            .FirstOrDefaultAsync(a => a.EmailAddress.ToLower() == email.ToLower());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in IsUsernameUsed()", email);
                throw ex;
            }
        }

        public Task<UserIdentity> GetUserFromUserId(string userId)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    return context.UserIdentities.FirstOrDefaultAsync(a => a.UserId.ToLower() == userId.ToLower());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserFromUserId()", userId);
                throw ex;
            }
        }

        public async Task<bool> IsEmailUsed(string email)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    return await context.UserIdentities.AnyAsync(a => a.EmailAddress.ToLower() == email.ToLower()) ;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in IsEmailUsed()", email);
                throw ex;
            }
        }

        public async Task<bool> ApiUserExists(string username)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    return await context.ApiUsers.AnyAsync(a => a.Username.ToLower() == username.ToLower());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in IsEmailUsed()", username);
                throw ex;
            }
        }

        public async Task<ApiUser> GetUserFromUsername(string username)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    return await context.ApiUsers
                          .Include(x => x.DiscordProfiles)
                        .FirstOrDefaultAsync(a => a.Username.ToLower() == username.ToLower());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in IsEmailUsed()", username);
                throw ex;
            }
        }

        public async Task<UnicityResult> SetTwitterProfile(ApiUser user)
        {
            try
            {
                var unicityResult = await _unicityConstraintsChecker.CheckUnicity(user, UnicityTopic.Twitter);
                if (unicityResult.AllGood)
                {
                    using (var context = _contextProvider.CoinstantineContext)
                    {
                        var savedUser = await context.ApiUsers.FirstOrDefaultAsync(x => x.Username == user.Username);
                        savedUser.TwitterProfile = user.TwitterProfile;
                        context.ApiUsers.Update(savedUser);
                        await context.SaveChangesAsync();
                    }
                }
                return unicityResult;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in SetTwitterProfile()", user);
                throw ex;
            }
        }

        public async Task<ApiUser> GetUserFromEmail(string email)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var user = await context.UserIdentities.FirstOrDefaultAsync(x => x.EmailAddress == email);
                    if (user != null)
                    {
                        return await context.ApiUsers.Include(a => a.Telegram)
                                                   .Include(a => a.TwitterProfile)
                                                   .Include(a => a.BctProfile)
                                                   .Include(a => a.BlockchainInfo)
                                                   .Include(a => a.DiscordProfiles)
                                                   .FirstOrDefaultAsync(x => x.Username == user.Username);
                    }
                    return null;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserFromEmail()", email);
                throw ex;
            }
        }

        public async Task<UnicityResult> SetTelegramProfile(ApiUser user)
        {
            try
            {
                var unicityResult = await _unicityConstraintsChecker.CheckUnicity(user, UnicityTopic.Telegram);
                if (unicityResult.AllGood)
                {
                    using (var context = _contextProvider.CoinstantineContext)
                    {
                        var savedUser = await context.ApiUsers.FirstOrDefaultAsync(x => x.Username == user.Username);
                        savedUser.Telegram = user.Telegram;
                        savedUser.Phonenumber = user.Phonenumber;
                        context.ApiUsers.Update(savedUser);
                        await context.SaveChangesAsync();
                    }
                }
                return unicityResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SetTelegramProfile()", user);
                throw ex;
            }
        }

        public async Task<BlockchainUser> GetBlockchainUserFromEmail(string email)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var user = await context.UserIdentities.FirstAsync(x => x.EmailAddress == email);
                    return await context.BlockchainUsers.FirstAsync(x => x.Username == user.Username);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetBlockchainUserFromEmail()", email);
                throw ex;
            }
        }

        public async Task<UnicityResult> SaveProfile(ApiUser user)
        {
            try
            {
                var unicityResult = await _unicityConstraintsChecker.CheckUnicity(user, UnicityTopic.Profile);
                if (unicityResult.AllGood)
                {
                    using (var context = _contextProvider.CoinstantineContext)
                    {
                        await context.ApiUsers.AddOrUpdateAsync(user);
                        await context.SaveChangesAsync();
                    }
                }
                return unicityResult;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in SaveProfile()", user);
                throw ex;
            }
        }

        public async Task<UnicityResult> SetBctProfile(ApiUser user)
        {
            try
            {
                var unicityResult = await _unicityConstraintsChecker.CheckUnicity(user, UnicityTopic.BitcoinTalk);
                if (unicityResult.AllGood)
                {
                    using (var context = _contextProvider.CoinstantineContext)
                    {
                        var savedUser = await context.ApiUsers.FirstOrDefaultAsync(x => x.Username == user.Username);
                        savedUser.BctProfile = user.BctProfile;
                        context.ApiUsers.Update(savedUser);
                        await context.SaveChangesAsync();
                    }
                }
                return unicityResult;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in SetBctProfile()", user);
                throw ex;
            }
        }

        public Task UpdateBctProfile(BitcoinTalkProfile bitcoinTalkProfile)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    context.BitcoinTalkProfiles.Update(bitcoinTalkProfile);
                    return context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateBctProfile()", bitcoinTalkProfile);
                throw ex;
            }
        }

        public Task UpdateTwitterProfile(TwitterProfile twitterProfile)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    context.TwitterProfiles.Update(twitterProfile);
                    return context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateTwitterProfile()", twitterProfile);
                throw ex;
            }
        }

        public Task<bool> UserExists(string username)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    return context.UserIdentities.AnyAsync(x => x.Username == username);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in UserExists()", username);
                throw ex;
            }
        }

        public async Task<bool> RemoveBctProfile(ApiUser user)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var profile = user.BctProfile;
                    context.BitcoinTalkProfiles.Remove(profile);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in RemoveBctProfile()", user);
                return false;
            }
        }

        public async Task<bool> RemoveTwitterProfile(ApiUser user)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var profile = user.TwitterProfile;
                    context.TwitterProfiles.Remove(profile);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in RemoveTwitterProfile()", user);
                return false;
            }
        }

        public async Task<bool> RemoveTelegramProfile(ApiUser user)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var profile = user.Telegram;
                    context.TelegramProfiles.Remove(profile);
                    user.Phonenumber = null;
                    context.ApiUsers.Update(user);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RemoveTelegramProfile()", user);
                return false;
            }
        }

        public async Task<bool> AddDiscordProfile(ApiUser user, DiscordProfile discordProfile)
        {
            try
            {
                var exists = _unicityConstraintsChecker.CheckDiscordUnicity(discordProfile);
                if (!exists)
                {
                    using (var context = _contextProvider.CoinstantineContext)
                    {
                        var savedUser = await context.ApiUsers.FirstOrDefaultAsync(x => x.Username == user.Username);
                        savedUser.DiscordProfiles = savedUser.DiscordProfiles ?? new List<DiscordProfile>();
                        savedUser.DiscordProfiles.Add(discordProfile);
                        context.ApiUsers.Update(savedUser);
                        await context.SaveChangesAsync();
                    }
                }
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddDiscordProfile()", user);
                throw ex;
            }
        }

        public async Task<ApiUser> GetUserFromDiscord(string callerId, string serverName)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                var discordProfile = await context.DiscordProfiles
                                            .Include(x => x.ApiUser)
                                                .FirstOrDefaultAsync(x => x.DiscordUserIdentifier == callerId && x.DiscordServerName == serverName);
                if(discordProfile?.ApiUser == null)
                {
                    discordProfile = await context.DiscordProfiles
                                            .Include(x => x.ApiUser)
                                                .FirstOrDefaultAsync(x => x.Username == callerId && x.DiscordServerName == serverName);
                    if (discordProfile?.ApiUser == null)
                    {
                        return null;
                    }
                }

                return await context.ApiUsers.Include(a => a.Telegram)
                                                   .Include(a => a.TwitterProfile)
                                                   .Include(a => a.BctProfile)
                                                   .Include(a => a.BlockchainInfo)
                                                   .Include(a => a.DiscordProfiles)
                                                   .FirstOrDefaultAsync(x => x.Id == discordProfile.ApiUser.Id);
            }
        }
    }
}
