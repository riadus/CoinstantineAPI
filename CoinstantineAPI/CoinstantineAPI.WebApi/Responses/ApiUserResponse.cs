using System.Collections.Generic;

namespace CoinstantineAPI.WebApi.Responses
{
    public class ApiUserResponse
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Phonenumber { get; set; }
        public string Culture { get; set; }
        public string DeviceId { get; set; }
        public string UniqueId { get; set; }
        public TwitterProfileResponse TwitterProfile { get; set; }
        public TelegramProfileResponse Telegram { get; set; }
        public BitcoinTalkProfileResponse BctProfile { get; set; }
        public BlockchainInfoResponse BlockchainInfo { get; set; }
        public List<DiscordProfileResponse> DiscordProfiles { get; set; }
    }
}
