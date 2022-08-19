using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoinstantineAPI.Data
{
    public class ApiUser : Entity
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Phonenumber { get; set; }
        public string Culture { get; set; }
        public string DeviceId { get; set; }
        public string UniqueId { get; set; }
        [ForeignKey("TwitterProfileId")]
        public TwitterProfile TwitterProfile { get; set; }
        [ForeignKey("TelegramId")]
        public TelegramProfile Telegram { get; set; }
        [ForeignKey("BctProfileId")]
        public BitcoinTalkProfile BctProfile { get; set; }
        [ForeignKey("BlockchainInfoId")]
        public BlockchainInfo BlockchainInfo { get; set; }

        public List<DiscordProfile> DiscordProfiles { get; set; }

        public UserIdentity UserIdentity { get; set; }
    }
}
