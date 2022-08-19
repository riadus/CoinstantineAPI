using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.TwitterProvider
{
    public interface IRandomTweetsProvider
    {
        Task<Tweet> GetRandomTweet(string language);
        void SetLoadFunc(Func<Task<IEnumerable<Tweet>>> loadFunc);
        Task ForceReload();
        Task<IEnumerable<Tweet>> GetListOfTweetsForReferral(string language);
    }
}
