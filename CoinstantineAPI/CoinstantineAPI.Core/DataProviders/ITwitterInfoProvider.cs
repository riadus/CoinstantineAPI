using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.DataProvider
{
    public interface ITwitterInfoProvider
    {
        Task<TwitterProfile> CheckTweetAndGetTwitterProfile(long tweetId, string screenName, long twitterAccountId);
        Task<TwitterProfile> GetTwitterProfile(long userId);
    }
}
