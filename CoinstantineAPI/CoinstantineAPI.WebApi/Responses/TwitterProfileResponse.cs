using System;

namespace CoinstantineAPI.WebApi.Responses
{
    public class TwitterProfileResponse
    {
        public string ScreenName { get; set; }
        public long TwitterId { get; set; }
        public int NumberOfFollower { get; set; }
        public DateTime CreationDate { get; set; }
        public string Username { get; set; }
        public bool Validated { get; set; }
        public DateTime? ValidationDate { get; set; }
    }
}
