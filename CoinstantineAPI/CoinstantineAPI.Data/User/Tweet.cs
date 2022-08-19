using System;

namespace CoinstantineAPI.Data
{
    public class Tweet : Entity
    {
        public string Text { get; set; }
        public string Language { get; set; }
        public string IsTranslationKey { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime ExpirationDate { get; set; }
        public TweetType TweetType { get; set; }
    }

    public enum TweetType
    {
        ValidationTweet,
        ReferralTweet
    }

    public class TwitterConfig : Entity
    {
        public bool Follow { get; set; }
        public long AccountToFollow { get; set; }
        public bool Tweet { get; set; }
        public bool Retweet { get; set; }
        public long TweetIdToRetweet { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }

        public bool UseIntents { get; set; }
        public bool UseFollowIntent { get; set; }
        public bool UseTweetIntent { get; set; }
        public bool UseRetweetIntent { get; set; }

        public string FollowIntent { get; set; }
        public string TweetIntent { get; set; }
        public string RetweetIntent { get; set; }

        public string BaseFollowIntent { get; set; }
        public string BaseTweetIntent { get; set; }
        public string BaseRetweetIntent { get; set; }
    }
}
