using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using CoinstantineAPI.Aidrops;
using CoinstantineAPI.Aidrops.Requirements.Interfaces;
using CoinstantineAPI.Core.Airdrops;
using CoinstantineAPI.Data;
using CoinstantineAPI.Tests.Builders;
using Xunit;

namespace CoinstantineAPI.Tests.AirdropTest
{
    public class AirdropServiceTests
    {
        private IEnumerable<AirdropDefinition> GetAirdropDefinitions()
        {
            var twitterRequirement1 = new TwitterAirdropRequirement
            {
                HasAccount = true,
                MinimumFollowers = 10,
                MinimumCreationDate = new DateTime(2018, 1, 1),
            };
            var twitterRequirement2 = new TwitterAirdropRequirement
            {
                HasAccount = true,
                MinimumFollowers = 10,
                MinimumCreationDate = new DateTime(2018, 1, 1),
            };
            var bctRequirement1 = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                MinimumRank = BitcoinTalkRank.Newbie,
                MinimumCreationDate = new DateTime(2018, 1, 1)
            };
            var bctRequirement2 = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                MinimumRank = BitcoinTalkRank.SrMember,
                MinimumCreationDate = new DateTime(2018, 1, 1)
            };
            var telegramRequirement1 = new TelegramAirdropRequirement
            {
                HasAccount = true
            };
            var telegramRequirement2 = new TelegramAirdropRequirement
            {
                HasAccount = true
            };

            var airdropDefinition1 = new AirdropDefinition
            {
                AirdropName = "Coinstantine Airdrop #1",
                BitcoinTalkAirdropRequirement = bctRequirement1,
                TelegramAirdropRequirement = telegramRequirement1,
                TwitterAirdropRequirement = twitterRequirement1,
                MaxLimit = 3,
                ExpirationDate = new DateTime(2020, 10, 1),
                TokenName = "Coinstantine",
                OtherInfoToDisplay = "Welcome pack",
                Amount = 100
            };

            var airdropDefinition2 = new AirdropDefinition
            {
                AirdropName = "Coinstantine Airdrop #2",
                BitcoinTalkAirdropRequirement = bctRequirement2,
                TelegramAirdropRequirement = telegramRequirement2,
                TwitterAirdropRequirement = twitterRequirement2,
                ExpirationDate = new DateTime(2020, 10, 1),
                TokenName = "Coinstantine",
                OtherInfoToDisplay = "Welcome pack for Sr. Members, minimum",
                Amount = 50,
                MaxLimit = 3
            };

            return new List<AirdropDefinition> { airdropDefinition1, airdropDefinition2 };
        }

        [Fact]
        public async Task First_Subscription_Should_Succeed()
        {
            var user = UserBuilder.ApiUsers[0];
            var requirementService = A.Fake<IRequirementToLambda>();
            A.CallTo(() => requirementService.MeetsAllRequirement(user, A<IEnumerable<IAirdropRequirement>>.Ignored))
             .Returns(true);
            var airdropService = new AirdropServiceBuilder()
                                    .WithRequirementToLambda(requirementService)
                                    .Build();
            var airdropDefinition = GetAirdropDefinitions().ElementAt(0);

            await airdropService.CreateAirdrop(airdropDefinition);

            var result = await airdropService.SubscribeToAirdrop(user, airdropDefinition.Id);

            var subscription = await airdropService.GetAirdropSubscription(airdropDefinition.Id);
            Assert.Equal(user.Id, subscription.Subscribers.ElementAt(0).UserId);
            Assert.True(result.Success);
            Assert.Equal(FailReason.None, result.FailReason);
        }

        [Fact]
        public async Task Second_Subscription_To_Same_Airdrop_Should_Fail()
        {
            var user = UserBuilder.ApiUsers[0];
            var requirementService = A.Fake<IRequirementToLambda>();
            A.CallTo(() => requirementService.MeetsAllRequirement(user, A<IEnumerable<IAirdropRequirement>>.Ignored))
             .Returns(true);
            var airdropService = new AirdropServiceBuilder()
                                    .WithRequirementToLambda(requirementService)
                                    .Build();
            var airdropDefinition = GetAirdropDefinitions().ElementAt(0);

            await airdropService.CreateAirdrop(airdropDefinition);

            await airdropService.SubscribeToAirdrop(user, airdropDefinition.Id);
            var subscriptionResult = await airdropService.SubscribeToAirdrop(user, airdropDefinition.Id);
            Assert.False(subscriptionResult.Success);
            Assert.Equal(FailReason.AlreadySubscribed, subscriptionResult.FailReason);
        }

        [Fact]
        public async Task Subscription_To_A_Full_Airdrop_Should_Fail()
        {
            var user = UserBuilder.ApiUsers[0];
            var requirementService = A.Fake<IRequirementToLambda>();
            A.CallTo(() => requirementService.MeetsAllRequirement(user, A<IEnumerable<IAirdropRequirement>>.Ignored))
             .Returns(true);
            var airdropService = new AirdropServiceBuilder()
                                    .WithRequirementToLambda(requirementService)
                                    .Build();
            var airdropDefinition = GetAirdropDefinitions().ElementAt(0);
            airdropDefinition.MaxLimit = 0;
            await airdropService.CreateAirdrop(airdropDefinition);

            var subscriptionResult = await airdropService.SubscribeToAirdrop(user, airdropDefinition.Id);
            Assert.False(subscriptionResult.Success);
            Assert.Equal(FailReason.Full, subscriptionResult.FailReason);
        }

        [Fact]
        public async Task Subscription_To_An_Expired_Airdrop_Should_Fail()
        {
            var user = UserBuilder.ApiUsers[0];
            var requirementService = A.Fake<IRequirementToLambda>();
            A.CallTo(() => requirementService.MeetsAllRequirement(user, A<IEnumerable<IAirdropRequirement>>.Ignored))
             .Returns(true);
            var airdropService = new AirdropServiceBuilder()
                                    .WithRequirementToLambda(requirementService)
                                    .Build();
            var airdropDefinition = GetAirdropDefinitions().ElementAt(0);
            airdropDefinition.ExpirationDate = DateTime.Now.AddDays(-1);
            await airdropService.CreateAirdrop(airdropDefinition);

            var subscriptionResult = await airdropService.SubscribeToAirdrop(user, airdropDefinition.Id);
            Assert.False(subscriptionResult.Success);
            Assert.Equal(FailReason.Expired, subscriptionResult.FailReason);
        }

        [Fact]
        public async Task Subscription_To_A_Not_Started_Airdrop_Should_Fail()
        {
            var user = UserBuilder.ApiUsers[0];
            var requirementService = A.Fake<IRequirementToLambda>();
            A.CallTo(() => requirementService.MeetsAllRequirement(user, A<IEnumerable<IAirdropRequirement>>.Ignored))
             .Returns(true);
            var airdropService = new AirdropServiceBuilder()
                                    .WithRequirementToLambda(requirementService)
                                    .Build();
            var airdropDefinition = GetAirdropDefinitions().ElementAt(0);
            airdropDefinition.StartDate = DateTime.Now.AddDays(1);
            await airdropService.CreateAirdrop(airdropDefinition);

            var subscriptionResult = await airdropService.SubscribeToAirdrop(user, airdropDefinition.Id);
            Assert.False(subscriptionResult.Success);
            Assert.Equal(FailReason.NotStarted, subscriptionResult.FailReason);
        }

        [Fact]
        public async Task Subscription_To_An_Unkown_Airdrop_Should_Fail()
        {
            var user = UserBuilder.ApiUsers[0];
            var requirementService = A.Fake<IRequirementToLambda>();
            A.CallTo(() => requirementService.MeetsAllRequirement(user, A<IEnumerable<IAirdropRequirement>>.Ignored))
             .Returns(true);
            var airdropService = new AirdropServiceBuilder()
                                    .WithRequirementToLambda(requirementService)
                                    .Build();
            var airdropDefinition = GetAirdropDefinitions().ElementAt(0);
            await airdropService.CreateAirdrop(airdropDefinition);

            var subscriptionResult = await airdropService.SubscribeToAirdrop(user, 4);
            Assert.False(subscriptionResult.Success);
            Assert.Equal(FailReason.UnknownAirdrop, subscriptionResult.FailReason);
        }

        [Fact]
        public async Task Subscription_To_Different_Airdrops_Should_Succeed()
        {
            var user = UserBuilder.ApiUsers[0];
            var requirementService = A.Fake<IRequirementToLambda>();
            A.CallTo(() => requirementService.MeetsAllRequirement(user, A<IEnumerable<IAirdropRequirement>>.Ignored))
             .Returns(true);
            var airdropService = new AirdropServiceBuilder()
                                    .WithRequirementToLambda(requirementService)
                                    .Build();
            var airdropDefinitions = GetAirdropDefinitions();

            var airdrop1 = airdropDefinitions.ElementAt(0);
            var airdrop2 = airdropDefinitions.ElementAt(1);

            await airdropService.CreateAirdrop(airdrop1);
            var result1 = await airdropService.SubscribeToAirdrop(user, airdrop1.Id);

            await airdropService.CreateAirdrop(airdrop2);
            var result2 = await airdropService.SubscribeToAirdrop(user, airdrop2.Id);

            var userAirdrops = await airdropService.GetUserAidrops(user);

            AssertExtentions.Count(userAirdrops.AirdropIds, 2);
            Assert.Equal(userAirdrops.UserId, user.Id);
            Assert.True(result1.Success);
            Assert.True(result2.Success);
            Assert.Equal(FailReason.None, result1.FailReason);
            Assert.Equal(FailReason.None, result2.FailReason);
        }

        [Fact]
        public async Task Different_Users_Subscribing_To_Different_Airdrops_Should_Succeed()
        {
            var user = UserBuilder.ApiUsers[0];
            var user2 = UserBuilder.ApiUsers[1];
            var requirementService = A.Fake<IRequirementToLambda>();

            A.CallTo(() => requirementService.MeetsAllRequirement(user, A<IEnumerable<IAirdropRequirement>>.Ignored)).Returns(true);
            A.CallTo(() => requirementService.MeetsAllRequirement(user2, A<IEnumerable<IAirdropRequirement>>.Ignored)).Returns(true);

            var airdropService = new AirdropServiceBuilder()
                                    .WithRequirementToLambda(requirementService)
                                    .Build();
            var airdropDefinitions = GetAirdropDefinitions();
            var airdrop1 = airdropDefinitions.ElementAt(0);
            var airdrop2 = airdropDefinitions.ElementAt(1);

            await airdropService.CreateAirdrop(airdrop1);
            await airdropService.CreateAirdrop(airdrop2);

            var result1 = await airdropService.SubscribeToAirdrop(user, airdrop1.Id);
            var result2 = await airdropService.SubscribeToAirdrop(user2, airdrop2.Id);

            var userAirdrops = await airdropService.GetUserAidrops(user);
            var userAirdrops2 = await airdropService.GetUserAidrops(user2);

            AssertExtentions.Count(userAirdrops.AirdropIds, 1);
            Assert.Equal(userAirdrops.UserId, user.Id);

            AssertExtentions.Count(userAirdrops2.AirdropIds, 1);
            Assert.Equal(userAirdrops2.UserId, user2.Id);

            Assert.True(result1.Success);
            Assert.True(result2.Success);
            Assert.Equal(FailReason.None, result1.FailReason);
            Assert.Equal(FailReason.None, result2.FailReason);
        }

        [Fact]
        public async Task Different_Users_Subscribing_To_Same_Airdrops_Should_Succeed()
        {
            var user = UserBuilder.ApiUsers[0];
            var user2 = UserBuilder.ApiUsers[1];
            var requirementService = A.Fake<IRequirementToLambda>();

            A.CallTo(() => requirementService.MeetsAllRequirement(user, A<IEnumerable<IAirdropRequirement>>.Ignored)).Returns(true);
            A.CallTo(() => requirementService.MeetsAllRequirement(user2, A<IEnumerable<IAirdropRequirement>>.Ignored)).Returns(true);

            var airdropService = new AirdropServiceBuilder()
                                    .WithRequirementToLambda(requirementService)
                                    .Build();
            var airdropDefinitions = GetAirdropDefinitions();
            var airdrop = airdropDefinitions.ElementAt(0);

            await airdropService.CreateAirdrop(airdrop);

            var result1 = await airdropService.SubscribeToAirdrop(user, airdrop.Id);
            var result2 = await airdropService.SubscribeToAirdrop(user2, airdrop.Id);

            var userAirdrops = await airdropService.GetUserAidrops(user);
            var userAirdrops2 = await airdropService.GetUserAidrops(user2);
            var airdropSubscription = await airdropService.GetAirdropSubscription(airdrop.Id);

            AssertExtentions.Count(userAirdrops.AirdropIds, 1);
            Assert.Equal(userAirdrops.UserId, user.Id);

            Assert.Equal(airdrop.Id, airdropSubscription.AirdropDefinition.Id);
            Assert.Equal(airdropSubscription.Count, 2);
            Assert.Equal(airdropSubscription.Subscribers.ElementAt(0).UserId, user.Id);
            Assert.Equal(airdropSubscription.Subscribers.ElementAt(1).UserId, user2.Id);
            Assert.Equal(airdropSubscription.Count, 2);
            AssertExtentions.Count(airdropSubscription.Subscribers, 2);

            AssertExtentions.Count(userAirdrops2.AirdropIds, 1);
            Assert.Equal(userAirdrops2.UserId, user2.Id);

            Assert.True(result1.Success);
            Assert.True(result2.Success);
            Assert.Equal(FailReason.None, result1.FailReason);
            Assert.Equal(FailReason.None, result2.FailReason);
        }

        [Fact]
        public async Task Subscription_When_Requirement_Not_Met_Should_Fail()
        {
            var user = UserBuilder.ApiUsers[0];
            var requirementService = A.Fake<IRequirementToLambda>();
            A.CallTo(() => requirementService.MeetsAllRequirement(user, A<IEnumerable<IAirdropRequirement>>.Ignored))
             .Returns(false);
            var airdropService = new AirdropServiceBuilder()
                                    .WithRequirementToLambda(requirementService)
                                    .Build();
            var airdropDefinition = GetAirdropDefinitions().ElementAt(0);

            await airdropService.CreateAirdrop(airdropDefinition);

            var result = await airdropService.SubscribeToAirdrop(user, airdropDefinition.Id);

            var subscription = await airdropService.GetAirdropSubscription(airdropDefinition.Id);
            AssertExtentions.Count(subscription.Subscribers, 0);
            Assert.False(result.Success);
            Assert.Equal(FailReason.RequirementsNotMet, result.FailReason);
        }
    }
}
