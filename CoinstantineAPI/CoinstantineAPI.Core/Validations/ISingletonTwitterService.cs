using System.Collections.Generic;
using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.Validations
{
    public interface ISingletonTwitterService
    {
        Task<Tweet> GetRandomTweet(string language);
        Task<IEnumerable<Tweet>> GetListOfTweetsForReferral(string language);
        Task<TwitterConfig> GetTwitterConfig();
    }
}
