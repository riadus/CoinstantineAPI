using System;

namespace CoinstantineAPI.Data
{
    public class TelegramProfile : Entity, IProfileItem
    {
        public string Username { get; set; }
        public long TelegramId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Validated { get; set; }
        public DateTime? ValidationDate { get; set; }

        public ApiUser ApiUser { get; set; }
    }
}
