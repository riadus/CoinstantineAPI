using System;

namespace CoinstantineAPI.WebApi.Responses
{
    public class TelegramProfileResponse
    {
        public string Username { get; set; }
        public long TelegramId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Validated { get; set; }
        public DateTime? ValidationDate { get; set; }
    }
}
