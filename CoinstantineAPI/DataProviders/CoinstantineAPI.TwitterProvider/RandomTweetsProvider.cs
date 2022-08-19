using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.TwitterProvider
{
    public class RandomTweetsProvider : IRandomTweetsProvider
    {
        private Func<Task<IEnumerable<Tweet>>> _loadFunction;
        private IEnumerable<Tweet> _possibleTweets;

        public RandomTweetsProvider()
        {
            _possibleTweets = new List<Tweet>();
        }

        public async Task ForceReload()
        {
            _possibleTweets = await _loadFunction();
        }

        public async Task<Tweet> GetRandomTweet(string language)
        {
            var tweets = await GetTweets(language, TweetType.ValidationTweet);
            var random = new Random();
            var tweetId = random.Next(tweets.Count());
            if (tweets.Count() > tweetId)
            {
                return tweets.ElementAt(tweetId);
            }
            return null;
        }

        private async Task InitializeIfNeeded()
        {
            if (!_possibleTweets.Any())
            {
                _possibleTweets = await _loadFunction();
            }
        }

        public void SetLoadFunc(Func<Task<IEnumerable<Tweet>>> loadFunc)
        {
            _loadFunction = loadFunc;
        }

        public async Task<IEnumerable<Tweet>> GetListOfTweetsForReferral(string language)
        {
            return await GetTweets(language, TweetType.ReferralTweet);
        }

        private async Task<IEnumerable<Tweet>> GetTweets(string language, TweetType tweetType)
        {
            await InitializeIfNeeded();
            var possibleTweets = string.IsNullOrEmpty(language) ? _possibleTweets : _possibleTweets.Where(x => x.Language == language);
            if (!possibleTweets.Any())
            {
                possibleTweets = _possibleTweets;
            }
            return possibleTweets.Where(x => x.TweetType == tweetType);
        }
    }
}
