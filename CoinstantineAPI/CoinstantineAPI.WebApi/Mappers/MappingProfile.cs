using AutoMapper;
using CoinstantineAPI.Core;
using CoinstantineAPI.Data;
using CoinstantineAPI.WebApi.DTO;
using CoinstantineAPI.WebApi.Responses;

namespace CoinstantineAPI.WebApi.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApiUserResponse, ApiUser>();
            CreateMap<TelegramProfileResponse, TelegramProfile>();
            CreateMap<TwitterProfileResponse, TwitterProfile>();
            CreateMap<BitcoinTalkProfileResponse, BitcoinTalkProfile>();
            CreateMap<DiscordProfileResponse, DiscordProfile>();
            CreateMap<SmartContractResponse, SmartContract>();
            CreateMap<TokenResponse, Token>();
            CreateMap<AirdropDefinitionResponse, AirdropDefinition>();
            CreateMap<TwitterAirdropRequirement, TwitterAirdropRequirement>();
            CreateMap<TelegramAirdropRequirement, TelegramAirdropRequirement>();
            CreateMap<BitcoinTalkAirdropRequirement, BitcoinTalkAirdropRequirement>();
            CreateMap<AirdropSubscriberResponse, AirdropSubscriber>();
            CreateMap<AirdropSubscriptionResponse, AirdropSubscription>();
            CreateMap<BuyTokensResultResponse, BuyTokensResult>();
            CreateMap<TransactionReceiptDataResponse, TransactionReceiptData>();
            CreateMap<AccountChangeViewModel, AccountChangeModel>();
            CreateMap<GameResponse, Game>();
            CreateMap<UserAchievementResponse, UserAchievement>();
            CreateMap<AchievementsResponse, Achievements>();
            CreateMap<TwitterConfigResponse, TwitterConfig>().ReverseMap().ForMember(x => x.TweetTextToTweet, opt => opt.Ignore());
            CreateMap<TweetResponse, Tweet>();
            CreateMap<Referral, ReferralResponse>()
                .ForMember(x => x.Host, opt => opt.MapFrom(x => Constants.WebsiteUrl));
        }
    }
}
