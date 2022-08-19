using System;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.Core.Extensions;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Core.Validations;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Users
{
    public class BitcoinTalkService : IBitcoinTalkService
    {
        private readonly IBitcoinTalkPublicProfileProvider _bitcoinTalkPublicProfileProvider;
        private readonly IUsersService _usersService;
        private readonly IContextProvider _contextProvider;

        public BitcoinTalkService(IBitcoinTalkPublicProfileProvider bitcoinTalkPublicProfileProvider,
                                  IUsersService usersService,
                                  IContextProvider contextProvider)
        {
            _bitcoinTalkPublicProfileProvider = bitcoinTalkPublicProfileProvider;
            _usersService = usersService;
            _contextProvider = contextProvider;
        }

        public Task StartProcess(long id)
        {
            return Task.FromResult(0);
        }

        public async Task<IProfileItem> GetProfileItem(long id)
        {
            return await _bitcoinTalkPublicProfileProvider.GetUser((int) id);
        }

        public bool IsTheUser(ApiUser user, long id)
        {
            return user.BctProfile.BctId == id;
        }

        public async Task<(IProfileItem Profile, bool Success)> SetProfileItem(ApiUser user, long id, object encapsulatedData = null)
        {
            var profile = await GetProfileItem(id) as BitcoinTalkProfile;
            profile.Validated = true;
            profile.ValidationDate = DateTime.Now;
            user.BctProfile = profile;
            var result = await _usersService.SetBctProfile(user);
            return (profile, result.AllGood);
        }

        public async Task<(IProfileItem Profile, bool Success)> Cancel(ApiUser user)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var profile = user.BctProfile;
                    if (profile.ValidationDate >= SystemTime.Now().AddMinutes(-2))
                    {
                        var successfullyRemoved = await _usersService.RemoveBctProfile(user);
                        return (null, successfullyRemoved);
                    }
                    return (profile, false);
                }
            }
            catch
            {
                return (null, false);
            }
        }

        public async Task<IProfileItem> Update(ApiUser user, long id)
        {
            if (user.BctProfile != null
               && user.BctProfile.Validated
               && user.BctProfile.BctId == id)
            {
                var profile = await GetProfileItem(id) as BitcoinTalkProfile;
                user.BctProfile.Activity = profile.Activity;
                user.BctProfile.Age = profile.Age;
                user.BctProfile.Aim = profile.Aim;
                user.BctProfile.Email = profile.Email;
                user.BctProfile.Icq = profile.Icq;
                user.BctProfile.LastActive = profile.LastActive;
                user.BctProfile.Msn = profile.Msn;
                user.BctProfile.Position = profile.Position;
                user.BctProfile.Location = profile.Location;
                user.BctProfile.Posts = profile.Posts;
                user.BctProfile.Trust = profile.Trust;
                user.BctProfile.Username = profile.Username;
                user.BctProfile.Website = profile.Website;
                user.BctProfile.Yim = profile.Yim;
                    
                await _usersService.UpdateBctProfile(user.BctProfile);
                return user.BctProfile;
            }
            return null;
        }

        public bool HasValidatedProfile(ApiUser user)
        {
            return user.BctProfile?.Validated ?? false;
        }
    }
}
