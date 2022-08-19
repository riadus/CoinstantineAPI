using System;
using System.Threading.Tasks;
using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.IntegrationTests.Mocks
{
    public class MockTwitterProvider : ITwitterInfoProvider
    {
        public Task<TwitterProfile> CheckTweetAndGetTwitterProfile(long tweetId, string screenName, long twitterAccountId)
        {
            return Task.FromResult(GetProfile((int)twitterAccountId));
        }

        public Task<TwitterProfile> GetTwitterProfile(long userId)
        {
            return Task.FromResult(GetProfile((int)userId));
        }

        private TwitterProfile GetProfile(int userId)
        {
            return new TwitterProfile
            {
                CreationDate = DateTime.Now.AddMonths(-userId),
                NumberOfFollower = userId*10,
                ScreenName = $"Mario Constantini {userId}",
                TwitterId = userId,
                Username = $"MarioConstantini_{userId}"
            };
        }
    }
}
