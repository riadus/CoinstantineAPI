using System;
using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.Core;
using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.Data;
using Tweetinvi;
using Tweetinvi.Models;

namespace CoinstantineAPI.DataProvider.TwitterProvider
{
    public class TwitterInfoProvider : ITwitterInfoProvider
    {
        public Task<TwitterProfile> GetTwitterProfile(long userId)
        {
            return Task.FromResult(GetTwittos(userId));
        }

        private TwitterProfile GetTwittos(long userId)
        {
            try
            {
                var accessToken = Constants.TwitterAccessToken;
                var accessTokenSecret = Constants.TwitterAccessTokenSecret;
                var key = Constants.TwitterKey;
                var secret = Constants.TwitterSecret;

                Auth.SetUserCredentials(key, secret, accessToken, accessTokenSecret);
                var twittos = User.GetUserFromId(userId);
                var twitterProfile = new TwitterProfile
                {
                    CreationDate = twittos.CreatedAt,
                    NumberOfFollower = twittos.FollowersCount,
                    ScreenName = twittos.ScreenName,
                    TwitterId = twittos.Id,
                    Username = twittos.Name
                };
                return twitterProfile;
            }
            catch(Exception e)
            {
                return null;
            }

        }

        public async Task<TwitterProfile> CheckTweetAndGetTwitterProfile(long tweetId, string screenName, long twitterAccountId)
        {
            if (await ValidateFollow(twitterAccountId))
            {
                var twittos = User.GetUserFromId(twitterAccountId);
                if(twittos == null)
                {
                    return null;
                }

                var twitterProfile = new TwitterProfile
                {
                    CreationDate = twittos.CreatedAt,
                    NumberOfFollower = twittos.FollowersCount,
                    ScreenName = twittos.ScreenName,
                    TwitterId = twittos.Id,
                    Username = twittos.Name
                };
                return twitterProfile;
            }
            return null;
        }

        private async Task<bool> ValidateFollow(long twitterAccountId)
        {
            var coinstantineTwitterAccount = User.GetUserFromId(1081185434548531200);
            var count = coinstantineTwitterAccount.FollowersCount;
            var followers = await coinstantineTwitterAccount.GetFollowerIdsAsync(count);
            return followers.Contains(twitterAccountId);
        }

        private bool Validate(ITweet tweet, string screenName, long twitterAccountId)
        {
            var valid = true;

            valid &= tweet?.Hashtags?.Any(hashtag => hashtag.Text == "CoinstantineApp") ?? false;
            valid &= tweet?.CreatedBy?.ScreenName == screenName;
            valid &= tweet?.CreatedBy?.Id == twitterAccountId;

            return valid;
        }
    }
}
