using System;
using System.Collections.Generic;
using System.Linq;
using CoinstantineAPI.Aidrops.Requirements.Interfaces;
using CoinstantineAPI.Core.Extensions;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Aidrops.Requirements
{
    public class RequirementToLambda : IRequirementToLambda
    {
        public IEnumerable<Requirement> GetRequirements(BitcoinTalkAirdropRequirement bitcoinTalkAirdropRequirement)
        {
            var result = new List<BitcoinTalkRequirement>
            {
                new BitcoinTalkRequirement
                {
                    MatchFunc = p => bitcoinTalkAirdropRequirement.HasAccount && (!p?.Username.IsNullOrEmpty() ?? false),
                    RequirementApplies = () => bitcoinTalkAirdropRequirement.HasAccountApplies,
                    Value = p => p?.Username
                },
                new BitcoinTalkRequirement
                {
                    MatchFunc = p => bitcoinTalkAirdropRequirement.MinimumActivity <=  p?.Activity,
                    RequirementApplies = () => bitcoinTalkAirdropRequirement.MinimumActivityApplies,
                    Value = p => bitcoinTalkAirdropRequirement.MinimumActivity.ToString()
                },
                new BitcoinTalkRequirement
                {
                    MatchFunc = p => bitcoinTalkAirdropRequirement.ExactRank == p?.Position,
                    RequirementApplies = () => bitcoinTalkAirdropRequirement.ExactRankApplies,
                    Value = p => bitcoinTalkAirdropRequirement.ExactRank.ToString()
                },
                new BitcoinTalkRequirement
                {
                    MatchFunc = p => bitcoinTalkAirdropRequirement.MinimumRank <= p?.Position,
                    RequirementApplies = () => bitcoinTalkAirdropRequirement.MinimumRankApplies,
                    Value = p => bitcoinTalkAirdropRequirement.MinimumRank.ToString()
                },
                new BitcoinTalkRequirement
                {
                    MatchFunc = p => bitcoinTalkAirdropRequirement.MinimumPosts <= p?.Posts,
                    RequirementApplies = () => bitcoinTalkAirdropRequirement.MinimumPostsApplies,
                    Value = p => bitcoinTalkAirdropRequirement.MinimumPosts.ToString()
                },
                new BitcoinTalkRequirement
                {
                    MatchFunc = p => bitcoinTalkAirdropRequirement.MinimumCreationDate >= p?.RegistrationDate && p?.RegistrationDate.Year >= 1900,
                    RequirementApplies = () => bitcoinTalkAirdropRequirement.MinimumCreationDateApplies,
                    Value = p => bitcoinTalkAirdropRequirement.MinimumCreationDate?.ToString("D")
                }
            };

            return result;
        }

        public IEnumerable<Requirement> GetRequirements(TelegramAirdropRequirement telegramAirdropRequirement)
        {
            var result = new List<TelegramRequirement>
            {
                new TelegramRequirement
                {
                    MatchFunc = p => telegramAirdropRequirement.HasAccount && (!p?.Username.IsNullOrEmpty() ?? false),
                    RequirementApplies = () => telegramAirdropRequirement.HasAccountApplies,
                    Value = p => p?.Username
                }
            };

            return result;
        }

        public IEnumerable<Requirement> GetRequirements(TwitterAirdropRequirement twitterAirdropRequirement)
        {
            var result = new List<TwitterRequirement>
            {
                new TwitterRequirement
                {
                    MatchFunc = p => twitterAirdropRequirement.HasAccount && (!p?.Username.IsNullOrEmpty() ?? false),
                    RequirementApplies = () => twitterAirdropRequirement.HasAccountApplies,
                    Value = p => p?.Username
                },
                new TwitterRequirement
                {
                    MatchFunc = new Func<TwitterProfile, bool>(p => twitterAirdropRequirement.MinimumFollowers <= p?.NumberOfFollower),
                    RequirementApplies = () => twitterAirdropRequirement.MinimumFollowersApplies,
                    Value = p => twitterAirdropRequirement.MinimumFollowers.ToString()
                },
                new TwitterRequirement
                {
                    MatchFunc = new Func<TwitterProfile, bool>(p => twitterAirdropRequirement.MinimumCreationDate >= p?.CreationDate && p?.CreationDate.Year > 2000),
                    RequirementApplies = () => twitterAirdropRequirement.MinimumCreationDateApplies,
                    Value = p => twitterAirdropRequirement.MinimumCreationDate?.ToString("D")
                }
            };

            return result;
        }

        public IEnumerable<Requirement> GetRequirements(DiscordAirdropRequirement discordAirdropRequirement)
        {
            var result = new List<DiscordRequirement>
            {
                new DiscordRequirement
                {
                    DiscordServerName = discordAirdropRequirement.ServerName,
                    MatchFunc = p => p?.DiscordServerName == discordAirdropRequirement.ServerName,
                    RequirementApplies = () => discordAirdropRequirement.NeedsToJoinServerApplies,
                    Value = p => p?.Username
                }
            };

            return result;
        }

        public IEnumerable<Requirement> GetRequirements(IAirdropRequirement requirement)
        {
            if (requirement is TwitterAirdropRequirement twitterRequirement)
            {
                return GetRequirements(twitterRequirement);
            }
            if (requirement is TelegramAirdropRequirement telegramRequirement)
            {
                return GetRequirements(telegramRequirement);
            }
            if (requirement is BitcoinTalkAirdropRequirement bitcoinTalkAirdropRequirement)
            {
                return GetRequirements(bitcoinTalkAirdropRequirement);
            }
            if (requirement is DiscordAirdropRequirement discordAirdropRequirement)
            {
                return GetRequirements(discordAirdropRequirement);
            }


            return default(IEnumerable<Requirement<IProfileItem>>);
        }

        public bool MeetsAllRequirement(ApiUser user, IEnumerable<IAirdropRequirement> requirements)
        {
            return requirements.Where(x => x != null).SelectMany(GetRequirements)
                                              .Select(requirement => MeetsRequirements(user, requirement))
                                              .All(x => x);
        }

        private bool MeetsTwitterRequirements(TwitterProfile twitterProfile, ITwitterRequirement twitterRequirement)
        {
            if (twitterRequirement.RequirementApplies())
                return twitterRequirement.MatchFunc(twitterProfile);
            return true;
        }

        private bool MeetsTelegramRequirements(TelegramProfile telegramProfile, ITelegramRequirement telegramRequirement)
        {
            if (telegramRequirement.RequirementApplies())
                return telegramRequirement.MatchFunc(telegramProfile);
            return true;
        }

        private bool MeetsBitcoinTalkRequirements(BitcoinTalkProfile bitcoinTalkProfile, IBitcoinTalkRequirement bitcoinTalkRequirement)
        {
            if (bitcoinTalkRequirement.RequirementApplies())
                return bitcoinTalkRequirement.MatchFunc(bitcoinTalkProfile);
            return true;
        }

        private bool MeetsDiscordRequirements(IEnumerable<DiscordProfile> discordProfiles, IDiscordRequirement discordRequirement)
        {
            if (discordRequirement.RequirementApplies())
            {
                var discordProfile = discordRequirement.GetProfileFromList(discordProfiles);
                return discordRequirement.MatchFunc(discordProfile);
            }
            return true;
        }

        private bool MeetsRequirements(ApiUser user, Requirement requirement)
        {
            if (requirement is ITwitterRequirement)
            {
                return MeetsTwitterRequirements(user.TwitterProfile, requirement as ITwitterRequirement);
            }
            if (requirement is ITelegramRequirement)
            {
                return MeetsTelegramRequirements(user.Telegram, requirement as ITelegramRequirement);
            }
            if (requirement is IBitcoinTalkRequirement)
            {
                return MeetsBitcoinTalkRequirements(user.BctProfile, requirement as IBitcoinTalkRequirement);
            }
            if (requirement is IDiscordRequirement)
            {
                return MeetsDiscordRequirements(user.DiscordProfiles, requirement as IDiscordRequirement);
            }
            return false;
        }
    }
}
