using System;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.WebApi.Responses
{
    public class BitcoinTalkProfileResponse
    {
        public int BctId { get; set; }
        public BitcoinTalkRank Position { get; set; }
        public int Posts { get; set; }
        public int Activity { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastActive { get; set; }
        public string Email { get; set; }
        public string Icq { get; set; }
        public string Aim { get; set; }
        public string Msn { get; set; }
        public string Yim { get; set; }
        public string Website { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
        public string Trust { get; set; }

        public bool Validated { get; set; }
        public DateTime? ValidationDate { get; set; }
        public string Username { get; set; }
    }
}
