using System;
using System.Collections.Generic;
using CoinstantineAPI.Data;
using CoinstantineAPI.Tests.Builders;
using Xunit;

namespace CoinstantineAPI.Tests.AirdropTest
{
    public class TelegramAirdropRequirementTests
    {
        [Fact]
        public void No_Telegram_Account_When_Needed_Should_Fail()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var telegramReq = new TelegramAirdropRequirement
            {
                HasAccount = true
            };
            var result = builder.MeetsAllRequirement(new ApiUser(), new List<IAirdropRequirement> { telegramReq });
            Assert.False(result);
        }

        [Fact]
        public void No_Telegram_Account_When_Not_Needed_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var telegramReq = new TelegramAirdropRequirement
            {
                HasAccount = false
            };
            var result = builder.MeetsAllRequirement(new ApiUser(), new List<IAirdropRequirement> { telegramReq });
            Assert.True(result);
        }

        [Fact]
        public void Having_Telegram_Account_Should_Succeed()
        {
            var builder = new RequirementToLambdaBuilder().Build();
            var telegramReq = new TelegramAirdropRequirement
            {
                HasAccount = true
            };
            var result = builder.MeetsAllRequirement(new ApiUser { Telegram = new TelegramProfile { Validated = true, Username = "@Account" } }, new List<IAirdropRequirement> { telegramReq });
            Assert.True(result);
        }

    }
}

