using System;
using System.Collections.Generic;
using CoinstantineAPI.Data;
using CoinstantineAPI.Tests.Builders;
using Xunit;

namespace CoinstantineAPI.Tests.AirdropTest
{
    public class BitcoinTalkAirdropRequirementTests
    {
        [Fact]
        public void No_BitcoinTalk_Account_When_Needed_Should_Fail()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true
            };
            var result = builder.MeetsAllRequirement(new ApiUser(), new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.False(result);
        }

        [Fact]
        public void No_BitcoinTalk_Account_When_Not_Needed_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = false
            };
            var result = builder.MeetsAllRequirement(new ApiUser(), new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Fact]
        public void Having_BitcoinTalk_Account_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true
            };
            var result = builder.MeetsAllRequirement(new ApiUser { BctProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account" }}, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Fact]
        public void Not_Having_Enough_Posts_When_Needed_Should_Fail()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                MinimumPosts = 100
            };
            var result = builder.MeetsAllRequirement(new ApiUser { BctProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Posts = 10 } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.False(result);
        }

        [Fact]
        public void Not_Having_Enough_Posts_When_Not_Needed_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
            };
            var result = builder.MeetsAllRequirement(new ApiUser { BctProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Posts = 10 } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Fact]
        public void Having_Enough_Posts_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                MinimumPosts = 100
            };
            var result = builder.MeetsAllRequirement(new ApiUser { BctProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Posts = 200 } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Fact]
        public void Not_Having_Enough_Activity_When_Needed_Should_Fail()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                MinimumActivity = 100
            };
            var result = builder.MeetsAllRequirement(new ApiUser { BctProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Activity = 10 } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.False(result);
        }

        [Fact]
        public void Not_Having_Enough_Activity_When_Not_Needed_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
            };
            var result = builder.MeetsAllRequirement(new ApiUser { BctProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Activity = 10 } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Fact]
        public void Having_Enough_Activity_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                MinimumActivity = 100
            };
            var result = builder.MeetsAllRequirement(new ApiUser { BctProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Activity = 200 } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Fact]
        public void Not_Having_A_Rank_High_Enough_When_Needed_Should_Fail()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                MinimumRank = BitcoinTalkRank.JrMember
            };
            var result = builder.MeetsAllRequirement(new ApiUser { BctProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Position = BitcoinTalkRank.Newbie } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.False(result);
        }

        [Fact]
        public void Not_Having_A_Rank_High_Enough_When_Not_Needed_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
            };
            var result = builder.MeetsAllRequirement(new ApiUser { BctProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account",Position = BitcoinTalkRank.Newbie } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Fact]
        public void Having_A_Rank_High_Enough_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                MinimumRank = BitcoinTalkRank.JrMember
            };
            var result = builder.MeetsAllRequirement(new ApiUser { BctProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Position = BitcoinTalkRank.SrMember } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Fact]
        public void Not_Having_The_Exact_Required_Rank_When_Needed_Should_Fail()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                ExactRank = BitcoinTalkRank.Newbie
            };
            var result = builder.MeetsAllRequirement(new ApiUser { BctProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Position = BitcoinTalkRank.JrMember } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.False(result);
        }

        [Fact]
        public void Not_Having_The_Exact_Required_Rank_When_Not_Needed_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
            };
            var result = builder.MeetsAllRequirement(new ApiUser { BctProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Position = BitcoinTalkRank.SrMember } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Fact]
        public void Having_The_Exact_Required_Rank_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                MinimumRank = BitcoinTalkRank.JrMember
            };
            var result = builder.MeetsAllRequirement(new ApiUser { BctProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Position = BitcoinTalkRank.JrMember } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Fact]
        public void Not_Having_An_Account_Created_Early_Enough_When_Needed_Should_Fail()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                MinimumCreationDate = new DateTime(2018, 1, 1)
            };
            var result = builder.MeetsAllRequirement(new ApiUser { BctProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", RegistrationDate = new DateTime(2018, 7, 1) } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.False(result);
        }

        [Fact]
        public void Not_Having_An_Account_Created_Early_Enough_When_Not_Needed_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true
            };
            var result = builder.MeetsAllRequirement(new ApiUser { BctProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", RegistrationDate = new DateTime(2018, 7, 1) } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Fact]
        public void Having_An_Account_Created_Early_Enough_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                MinimumCreationDate = new DateTime(2018, 1, 1)
            };
            var result = builder.MeetsAllRequirement(new ApiUser { BctProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", RegistrationDate = new DateTime(2017, 7, 1) } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }
    }
}
