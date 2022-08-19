using System;

namespace CoinstantineAPI.Core.Validations
{
    public class TwitterData
    {
        public long TwitterId { get; set; }
        public string ScreenName { get; set; }
        public long TweetId { get; set; }
        public string Username { get; set; }
        public int NumberOfFollower { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
