using System;
using System.Threading.Tasks;
using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.Core.Extensions;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using CoinstantineAPI.Tests.Builders;
using CoinstantineAPI.UnitTests;
using CoinstantineAPI.Users.Unicity;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace CoinstantineAPI.Tests.ThirdPartyTests
{
    public class BitcoinTalkServiceTests
    {
        private BitcoinTalkProfile BuildProfile()
        {
            return new BitcoinTalkProfile
            {
                BctId = 123,
                Username = "Satoshi"
            };
        }

        private BitcoinTalkProfile BuildUpdatedProfile()
        {
            return new BitcoinTalkProfile
            {
                BctId = 123,
                Username = "Satosho"
            };
        }

        [Fact]
        public async Task Get_Bct_User_Should_Return_BitcoinTalk_Public_Profile()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<IBitcoinTalkPublicProfileProvider>();

            A.CallTo(() => profileProvider.GetUser(123)).Returns(Task.FromResult(BuildProfile()));

            var bitcoinTalkService = new BitcoinTalkServiceBuilder()
                .WithContextProvider(contextProvider)
                .WithBitcoinTalkPublicProfileProvider(profileProvider)
                .Build();

            var profile = await bitcoinTalkService.GetProfileItem(123) as BitcoinTalkProfile;
            profile.BctId.Should().Be(123);
            profile.Validated.Should().BeFalse();
        }

        [Fact]
        public async Task Get_Bct_User_With_Unkown_Id_Should_Return_Null()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<IBitcoinTalkPublicProfileProvider>();

            A.CallTo(() => profileProvider.GetUser(123)).Returns(Task.FromResult<BitcoinTalkProfile>(null));

            var bitcoinTalkService = new BitcoinTalkServiceBuilder()
                .WithBitcoinTalkPublicProfileProvider(profileProvider)
                .WithContextProvider(contextProvider)
                .Build();

            var profile = await bitcoinTalkService.GetProfileItem(123) as BitcoinTalkProfile;
            profile.Should().BeNull();
        }

        [Fact]
        public async Task Set_Bct_User_Shoud_Save()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<IBitcoinTalkPublicProfileProvider>();
            var usersService = A.Fake<IUsersService>();
            var user = new ApiUser();
            A.CallTo(() => profileProvider.GetUser(123)).Returns(Task.FromResult(BuildProfile()));

            var bitcoinTalkService = new BitcoinTalkServiceBuilder()
                .WithUsersService(usersService)
                .WithBitcoinTalkPublicProfileProvider(profileProvider)
                .WithContextProvider(contextProvider)
                .Build();

            var (profileItem, success) = await bitcoinTalkService.SetProfileItem(user, 123);
            var bctProfile = profileItem as BitcoinTalkProfile;
            bctProfile.BctId.Should().Be(123);
            profileItem.Validated.Should().BeTrue();
            A.CallTo(() => usersService.SetBctProfile(user)).MustHaveHappened();
        }

        [Fact]
        public async Task Set_Bct_User_Shoud_Succeed()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<IBitcoinTalkPublicProfileProvider>();
            var unicityChecker = A.Fake<IUnicityConstraintsChecker>();
            var user = new ApiUser { Username = "Satoshi" };

            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.BitcoinTalk)).Returns(Task.FromResult(UnicityResult.True));
            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Profile)).Returns(Task.FromResult(UnicityResult.True));

            var usersService = new UsersServiceBuilder()
                .WithContextProvider(contextProvider)
                .WithUnicityConstraintsChecker(unicityChecker)
                .Build();
            
            await usersService.SaveProfile(user);

            A.CallTo(() => profileProvider.GetUser(123)).Returns(Task.FromResult(BuildProfile()));

            var bitcoinTalkService = new BitcoinTalkServiceBuilder()
                .WithUsersService(usersService)
                .WithBitcoinTalkPublicProfileProvider(profileProvider)
                .WithContextProvider(contextProvider)
                .Build();

            var (profileItem, success) = await bitcoinTalkService.SetProfileItem(user, 123);
            var bctProfile = profileItem as BitcoinTalkProfile;
            bctProfile.BctId.Should().Be(123);
            bctProfile.Validated.Should().BeTrue();
            success.Should().BeTrue();
        }

        [Fact]
        public async Task Canceling_Bct_Profile_Within_2_Minutes_Shoud_Succeed()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<IBitcoinTalkPublicProfileProvider>();
            var unicityChecker = A.Fake<IUnicityConstraintsChecker>();
            var user = new ApiUser { Username = "Satoshi" };

            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.BitcoinTalk)).Returns(Task.FromResult(UnicityResult.True));
            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Profile)).Returns(Task.FromResult(UnicityResult.True));


            var usersService = new UsersServiceBuilder()
                .WithUnicityConstraintsChecker(unicityChecker)
                .WithContextProvider(contextProvider)
                .Build();
            await usersService.SaveProfile(user);
            A.CallTo(() => profileProvider.GetUser(123)).Returns(Task.FromResult(BuildProfile()));

            var bitcoinTalkService = new BitcoinTalkServiceBuilder()
                .WithUsersService(usersService)
                .WithBitcoinTalkPublicProfileProvider(profileProvider)
                .WithContextProvider(contextProvider)
                .Build();

            var (profileItem, success) = await bitcoinTalkService.SetProfileItem(user, 123);
            var bctProfile = profileItem as BitcoinTalkProfile;
            bctProfile.BctId.Should().Be(123);
            success.Should().BeTrue();
            SystemTime.SetDateTime(DateTime.Now.AddMinutes(1));
            (profileItem, success) = await bitcoinTalkService.Cancel(user);
            success.Should().BeTrue();
            profileItem.Should().BeNull();
        }

        [Fact]
        public async Task Canceling_Bct_Profile_After_More_Than_2_Minutes_Shoud_Fail()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<IBitcoinTalkPublicProfileProvider>();
            var unicityChecker = A.Fake<IUnicityConstraintsChecker>();
            var user = new ApiUser { Username = "Satoshi" };

            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.BitcoinTalk)).Returns(Task.FromResult(UnicityResult.True));
            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Profile)).Returns(Task.FromResult(UnicityResult.True));


            var usersService = new UsersServiceBuilder()
                .WithUnicityConstraintsChecker(unicityChecker)
                .WithContextProvider(contextProvider)
                .Build();
            await usersService.SaveProfile(user);
            A.CallTo(() => profileProvider.GetUser(123)).Returns(Task.FromResult(BuildProfile()));

            var bitcoinTalkService = new BitcoinTalkServiceBuilder()
                .WithUsersService(usersService)
                .WithBitcoinTalkPublicProfileProvider(profileProvider)
                .WithContextProvider(contextProvider)
                .Build();

            var (profileItem, success) = await bitcoinTalkService.SetProfileItem(user, 123);
            var bctProfile = profileItem as BitcoinTalkProfile;
            bctProfile.BctId.Should().Be(123);
            success.Should().BeTrue();
            SystemTime.SetDateTime(DateTime.Now.AddMinutes(3));
            (profileItem, success) = await bitcoinTalkService.Cancel(user);
            bctProfile = profileItem as BitcoinTalkProfile;
            success.Should().BeFalse();
            profileItem.Should().NotBeNull();
            bctProfile.BctId.Should().Be(123);
        }

        [Fact]
        public async Task Updating_A_Validated_Bct_Profile_Shoud_Succeed()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<IBitcoinTalkPublicProfileProvider>();
            var unicityChecker = A.Fake<IUnicityConstraintsChecker>();
            var user = new ApiUser { Username = "Satoshi" };

            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.BitcoinTalk)).Returns(Task.FromResult(UnicityResult.True));
            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Profile)).Returns(Task.FromResult(UnicityResult.True));


            var usersService = new UsersServiceBuilder()
                .WithUnicityConstraintsChecker(unicityChecker)
                .WithContextProvider(contextProvider)
                .Build();
            await usersService.SaveProfile(user);
            A.CallTo(() => profileProvider.GetUser(123)).Returns(Task.FromResult(BuildProfile()));

            var bitcoinTalkService = new BitcoinTalkServiceBuilder()
                .WithUsersService(usersService)
                .WithBitcoinTalkPublicProfileProvider(profileProvider)
                .WithContextProvider(contextProvider)
                .Build();

            var (profileItem, success) = await bitcoinTalkService.SetProfileItem(user, 123);
            var bctProfile = profileItem as BitcoinTalkProfile;
            bctProfile.BctId.Should().Be(123);
            bctProfile.Username.Should().Be("Satoshi");
            bctProfile.Validated.Should().BeTrue();
            success.Should().BeTrue();

            A.CallTo(() => profileProvider.GetUser(123)).Returns(Task.FromResult(BuildUpdatedProfile()));

            profileItem = await bitcoinTalkService.Update(user, 123);
            bctProfile = profileItem as BitcoinTalkProfile;
            bctProfile.BctId.Should().Be(123);
            bctProfile.Username.Should().Be("Satosho");
        }
    }
}
