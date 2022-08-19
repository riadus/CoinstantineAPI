using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Validations;
using CoinstantineAPI.Data;
using CoinstantineAPI.TwitterProvider;
using Microsoft.EntityFrameworkCore;

namespace CoinstantineAPI.Users
{
    public class SingletonTwitterService : ISingletonTwitterService
    {
        private readonly IRandomTweetsProvider _randomTweetsProvider;
        private readonly IContextProvider _contextProvider;

        public SingletonTwitterService(IContextProvider contextProvider, IRandomTweetsProvider randomTweetsProvider)
        {
            _randomTweetsProvider = randomTweetsProvider;
            _contextProvider = contextProvider;
            _randomTweetsProvider.SetLoadFunc(GetTweets);
        }

        public Task<IEnumerable<Tweet>> GetListOfTweetsForReferral(string language)
        {
            return _randomTweetsProvider.GetListOfTweetsForReferral(language);
        }

        public Task<Tweet> GetRandomTweet(string language)
        {
            return _randomTweetsProvider.GetRandomTweet(language);
        }

        public async Task<TwitterConfig> GetTwitterConfig()
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                var configs = await context.TwitterConfigs.ToListAsync();
                if(!configs?.Any() ?? true)
                {
                    return null;
                }
                var random = new Random();
                var configId = random.Next(configs.Count());
                return configs[configId];
            }
        }


        private async Task<IEnumerable<Tweet>> GetTweets()
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                return await context.Tweets.Where(x => x.AvailableFrom <= DateTime.Now && x.ExpirationDate >= DateTime.Now).ToListAsync();
            }
        }

    }
}