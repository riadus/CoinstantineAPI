using System;
using System.Collections.Generic;
using CoinstantineAPI.Data;
using CoinstantineAPI.Tests.Builders;
using Xunit;

namespace CoinstantineAPI.Tests.AirdropTest
{
    public class TwitterAirdropRequirementTests
    {
        [Fact]
        public void No_Twitter_Account_When_Needed_Should_Fail()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = true,
            };
            var result = builder.MeetsAllRequirement(new ApiUser(), new List<IAirdropRequirement> { twitterReq });
            Assert.False(result);
        }

        [Fact]
        public void No_Twitter_Account_When_Not_Needed_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = false
            };
            var result = builder.MeetsAllRequirement(new ApiUser(), new List<IAirdropRequirement> { twitterReq });
            Assert.True(result);
        }

        [Fact]
        public void Having_Twitter_Account_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = true
            };
            var result = builder.MeetsAllRequirement(new ApiUser { TwitterProfile = new TwitterProfile { Validated = true, Username = "@Account" }}, new List<IAirdropRequirement> { twitterReq });
            Assert.True(result);
        }

        [Fact]
        public void Not_Having_Enough_Followers_When_Needed_Should_Fail()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = true,
                MinimumFollowers = 100
            };
            var result = builder.MeetsAllRequirement(new ApiUser { TwitterProfile = new TwitterProfile { Validated = true, Username = "@Account", NumberOfFollower = 10 } }, new List<IAirdropRequirement> { twitterReq });
            Assert.False(result);
        }

        [Fact]
        public void Not_Having_Enough_Followers_When_Not_Needed_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = true,
            };
            var result = builder.MeetsAllRequirement(new ApiUser { TwitterProfile = new TwitterProfile { Validated = true, Username = "@Account", NumberOfFollower = 10 } }, new List<IAirdropRequirement> { twitterReq });
            Assert.True(result);
        }

        [Fact]
        public void Having_Enough_Followers_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = true,
                MinimumFollowers = 100
            };
            var result = builder.MeetsAllRequirement(new ApiUser { TwitterProfile = new TwitterProfile { Validated = true, Username = "@Account", NumberOfFollower = 200 } }, new List<IAirdropRequirement> { twitterReq });
            Assert.True(result);
        }

        [Fact]
        public void Not_Having_An_Account_Created_Early_Enough_When_Needed_Should_Fail()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = true,
                MinimumCreationDate = new DateTime(2018, 1, 1)
            };
            var result = builder.MeetsAllRequirement(new ApiUser { TwitterProfile = new TwitterProfile { Validated = true, Username = "@Account", CreationDate = new DateTime(2018, 7, 1) } }, new List<IAirdropRequirement> { twitterReq });
            Assert.False(result);
        }

        [Fact]
        public void Not_Having_An_Account_Created_Early_Enough_When_Not_Needed_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = true
            };
            var result = builder.MeetsAllRequirement(new ApiUser { TwitterProfile = new TwitterProfile { Validated = true, Username = "@Account", CreationDate = new DateTime(2018, 7, 1) } }, new List<IAirdropRequirement> { twitterReq });
            Assert.True(result);
        }

        [Fact]
        public void Having_An_Account_Created_Early_Enough_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = true,
                MinimumCreationDate = new DateTime(2018, 1, 1)
            };
            var result = builder.MeetsAllRequirement(new ApiUser { TwitterProfile = new TwitterProfile { Validated = true, Username = "@Account", CreationDate = new DateTime(2017, 7, 1) } }, new List<IAirdropRequirement> { twitterReq });
            Assert.True(result);
        }
    }
}
