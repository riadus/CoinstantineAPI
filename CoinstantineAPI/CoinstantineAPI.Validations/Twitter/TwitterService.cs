using System;
using System.Threading.Tasks;
using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.Core.Extensions;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Core.Validations;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Users
{
    public class TwitterService : ITwitterService
    {
        private readonly ITwitterInfoProvider _twitterInfoProvider;
        private readonly IUsersService _usersService;

        public TwitterService(ITwitterInfoProvider twitterInfoProvider,
                              IUsersService usersService)
        {
            _twitterInfoProvider = twitterInfoProvider;
            _usersService = usersService;

        }

        public async Task<(IProfileItem Profile, bool Success)> Cancel(ApiUser user)
        {
            try
            {
                var profile = user.TwitterProfile;
                if (profile.ValidationDate >= SystemTime.Now().AddMinutes(-2))
                {
                    var successfullyRemoved = await _usersService.RemoveTwitterProfile(user);
                    return (null, successfullyRemoved);
                }
                return (profile, false);
            }
            catch
            {
                return (null, false);
            }
        }

        public async Task<IProfileItem> GetProfileItem(long id)
        {
            return await _twitterInfoProvider.GetTwitterProfile(id);
        }

        public bool IsTheUser(ApiUser user, long id)
        {
            return user.TwitterProfile.TwitterId == id;
        }

        public async Task<(IProfileItem Profile, bool Success)> SetProfileItem(ApiUser user, long id, object encapsulatedData = null)
        {
            var twitterData = encapsulatedData as TwitterData;
            if(twitterData == null)
            {
                return (null, false);
            }
            var profile = await _twitterInfoProvider.CheckTweetAndGetTwitterProfile(id, twitterData.ScreenName, twitterData.TwitterId);
            profile.Validated = true;
            profile.ValidationDate = DateTime.Now;
            user.TwitterProfile = profile;
            var result = await _usersService.SetTwitterProfile(user);
            return (profile, result.AllGood);
        }

        public Task StartProcess(long id)
        {
            return Task.FromResult(0);
        }

        public async Task<IProfileItem> Update(ApiUser user, long id)
        {
            if (user.TwitterProfile != null
                && user.TwitterProfile.Validated
                && user.TwitterProfile.TwitterId == id)
            {
                var profile = await GetProfileItem(id) as TwitterProfile;

                user.TwitterProfile.NumberOfFollower = profile.NumberOfFollower;
                user.TwitterProfile.ScreenName = profile.ScreenName;
                user.TwitterProfile.Username = profile.Username;

                await _usersService.UpdateTwitterProfile(user.TwitterProfile);
                return user.TwitterProfile;
            }
            return null;
        }

        public bool HasValidatedProfile(ApiUser user)
        {
            return user.TwitterProfile?.Validated ?? false;
        }


    }
}