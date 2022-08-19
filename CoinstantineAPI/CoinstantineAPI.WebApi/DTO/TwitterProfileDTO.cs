using System;

namespace CoinstantineAPI.WebApi.DTO
{
    public class TwitterProfileDTO
    {
        public long TwitterId { get; set; }
        public string ScreenName { get; set; }
        public long TweetId { get; set; }
        public string Username { get; set; }
        public int NumberOfFollower { get; set; }
        public DateTime CreationDate { get; set; }
    }
}


