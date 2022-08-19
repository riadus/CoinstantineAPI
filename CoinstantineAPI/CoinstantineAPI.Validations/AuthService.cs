using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CoinstantineAPI.Users
{
    public class AuthService : IAuthService
    {
        private readonly IContextProvider _contextProvider;
        private readonly IPasswordService _passwordService;

        public AuthService(IContextProvider contextProvider, IPasswordService passwordService)
        {
            _contextProvider = contextProvider;
            _passwordService = passwordService;
        }

        public async Task<UserIdentity> Authenticate(string username, string password)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                var user = await context.UserIdentities.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
                if(_passwordService.VerifyPasswordHash(password, user.Password, user.PasswordSalt))
                {
                    return user;
                }
            }
            return null;
        }

        public async Task CreateRefreshToken(UserIdentity user, RefreshTokens refreshToken, ApplicationClient applicationClient)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                user.RefreshTokens.Where(x => x.Application.ApplicationId == applicationClient.ClientId && x.User == user)
                                  .ToList()
                                  .ForEach(x => context.RefreshTokens.Remove(x));
                await context.SaveChangesAsync();

                var application = await context.Applications.FirstAsync(x => x.ApplicationId == applicationClient.ClientId);
                if (user.RefreshTokens == null)
                {
                    user.RefreshTokens = new List<RefreshTokens>();
                }
                refreshToken.Application = application;
                refreshToken.User = user;
                user.RefreshTokens.Add(refreshToken);
                context.UserIdentities.Update(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateRefreshToken(UserIdentity user, RefreshTokens refreshToken, ApplicationClient applicationClient)
        {
            var existingRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Application.ApplicationId == applicationClient.ClientId && x.User == user);
            refreshToken.ExpirationDate = existingRefreshToken?.ExpirationDate ?? refreshToken.ExpirationDate;

            await CreateRefreshToken(user, refreshToken, applicationClient);
        }

        public bool VerifyRefreshToken(UserIdentity user, string refreshToken, ApplicationClient applicationClient)
        {
            var refreshTokens = user.RefreshTokens.FirstOrDefault(x => x.Application.ApplicationId == applicationClient.ClientId);
            if(refreshTokens == null)
            {
                return false;
            }
            return refreshTokens.ExpirationDate >= DateTime.UtcNow && _passwordService.VerifyPasswordHash(refreshToken, refreshTokens.RefreshToken, refreshTokens.RefreshTokenSalt);
        }
    }
}
