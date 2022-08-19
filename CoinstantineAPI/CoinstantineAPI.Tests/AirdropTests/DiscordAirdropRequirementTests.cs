using System.Collections.Generic;
using CoinstantineAPI.Data;
using CoinstantineAPI.Tests.Builders;
using Xunit;

namespace CoinstantineAPI.Tests.AirdropTest
{
    public class DiscordAirdropRequirementTests
    {
        [Fact]
        public void Being_In_Discord_Channel_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var discordReq = new DiscordAirdropRequirement
            {
                NeedsToJoinServer = true,
                ServerUrl = "discordChannelUrl",
                ServerName = "discordChannel"
            };
            var result = builder.MeetsAllRequirement(new ApiUser { DiscordProfiles = new List<DiscordProfile> { new DiscordProfile { DiscordChannelUrl = "discordChannelUrl", DiscordServerName = "discordChannel" } } }
            , new List<IAirdropRequirement> { discordReq });
            Assert.True(result);
        }

        [Fact]
        public void Being_In_Several_Discord_Channels_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var discordReq = new DiscordAirdropRequirement
            {
                NeedsToJoinServer = true,
                ServerUrl = "discordChannelUrl",
                ServerName = "discordChannel"
            };
            var result = builder.MeetsAllRequirement(new ApiUser
            {
                DiscordProfiles = new List<DiscordProfile> { new DiscordProfile { DiscordChannelUrl = "discordChannelUrl", DiscordServerName = "discordChannel" }
                                                                                                               ,new DiscordProfile { DiscordChannelUrl = "discordChannelUrl2", DiscordServerName = "discordChannel2" } }
            }
            , new List<IAirdropRequirement> { discordReq });
            Assert.True(result);
        }

        [Fact]
        public void Being_In_No_Discord_Channels_Should_Fail_If_NeedsToJoinServer()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var discordReq = new DiscordAirdropRequirement
            {
                NeedsToJoinServer = true,
                ServerUrl = "discordChannelUrl",
                ServerName = "discordChannel"
            };
            var result = builder.MeetsAllRequirement(new ApiUser(), new List<IAirdropRequirement> { discordReq });
            Assert.False(result);
        }

        [Fact]
        public void Being_In_No_Discord_Channels_Should_Succeed_If_Not_NeedsToJoinServer()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var discordReq = new DiscordAirdropRequirement
            {
                NeedsToJoinServer = false
            };
            var result = builder.MeetsAllRequirement(new ApiUser(), new List<IAirdropRequirement> { discordReq });
            Assert.True(result);
        }

        [Fact]
        public void Being_In_Another_Discord_Channel_Should_Fail()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var discordReq = new DiscordAirdropRequirement
            {
                NeedsToJoinServer = true,
                ServerUrl = "discordChannelUrl",
                ServerName = "discordChannel"
            };
            var result = builder.MeetsAllRequirement(new ApiUser { DiscordProfiles = new List<DiscordProfile> { new DiscordProfile { DiscordChannelUrl = "discordChannelUrl2", DiscordServerName = "discordChannel2" } } }
            , new List<IAirdropRequirement> { discordReq });
            Assert.False(result);
        }
    }
}
