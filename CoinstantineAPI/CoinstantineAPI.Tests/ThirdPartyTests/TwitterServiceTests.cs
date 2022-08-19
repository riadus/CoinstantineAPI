using System;
using System.Threading.Tasks;
using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.Core.Extensions;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Core.Validations;
using CoinstantineAPI.Data;
using CoinstantineAPI.Tests.Builders;
using CoinstantineAPI.UnitTests;
using CoinstantineAPI.Users.Unicity;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace CoinstantineAPI.Tests.ThirdPartyTests
{
    public class TwitterServiceTests
    {
        private TwitterProfile BuildProfile()
        {
            return new TwitterProfile
            {
                TwitterId = 123,
                Username = "Satoshi",
                NumberOfFollower = 10
            };
        }

        private TwitterProfile BuildUpdatedProfile()
        {
            return new TwitterProfile
            {
                TwitterId = 123,
                Username = "Satoshi",
                NumberOfFollower = 20
            };
        }

        [Fact]
        public async Task Get_Twitter_User_Should_Return_Twitter_Public_Profile()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<ITwitterInfoProvider>();

            A.CallTo(() => profileProvider.GetTwitterProfile(123)).Returns(Task.FromResult(BuildProfile()));

            var twitterService = new TwitterServiceBuilder()
                .WithTwitterInfoProvider(profileProvider)
                .Build();

            var profile = await twitterService.GetProfileItem(123) as TwitterProfile;
            profile.TwitterId.Should().Be(123);
            profile.Validated.Should().BeFalse();
        }

        [Fact]
        public async Task Get_Twitter_User_With_Unkown_Id_Should_Return_Null()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<ITwitterInfoProvider>();

            A.CallTo(() => profileProvider.GetTwitterProfile(123)).Returns(Task.FromResult<TwitterProfile>(null));

            var twitterService = new TwitterServiceBuilder()
                .WithTwitterInfoProvider(profileProvider)
                .Build();

            var profile = await twitterService.GetProfileItem(123) as TwitterProfile;
            profile.Should().BeNull();
        }

        [Fact]
        public async Task Set_Twitter_User_Shoud_Save()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<ITwitterInfoProvider>();
            var usersService = A.Fake<IUsersService>();
            var user = new ApiUser();
            A.CallTo(() => profileProvider.CheckTweetAndGetTwitterProfile(123, "Satoshi", 12345)).Returns(Task.FromResult(BuildProfile()));

            var twitterService = new TwitterServiceBuilder()
                .WithUsersService(usersService)
                .WithTwitterInfoProvider(profileProvider)
                .Build();

            var (profileItem, success) = await twitterService.SetProfileItem(user, 123, new TwitterData { ScreenName = "Satoshi", TwitterId = 12345 });
            var twitterProfile = profileItem as TwitterProfile;
            twitterProfile.TwitterId.Should().Be(123);
            profileItem.Validated.Should().BeTrue();
            A.CallTo(() => usersService.SetTwitterProfile(user)).MustHaveHappened();
        }

        [Fact]
        public async Task Set_Twitter_User_Shoud_Succeed()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<ITwitterInfoProvider>();
            var unicityChecker = A.Fake<IUnicityConstraintsChecker>();
            var user = new ApiUser { Username = "Satoshi" };

            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Twitter)).Returns(Task.FromResult(UnicityResult.True));
            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Profile)).Returns(Task.FromResult(UnicityResult.True));

            var usersService = new UsersServiceBuilder()
                .WithUnicityConstraintsChecker(unicityChecker)
                .WithContextProvider(contextProvider)
                .Build();
            await usersService.SaveProfile(user);

            A.CallTo(() => profileProvider.CheckTweetAndGetTwitterProfile(123, "Satoshi", 12345)).Returns(Task.FromResult(BuildProfile()));

            var twitterService = new TwitterServiceBuilder()
                .WithUsersService(usersService)
                .WithTwitterInfoProvider(profileProvider)
                .Build();

            var (profileItem, success) = await twitterService.SetProfileItem(user, 123, new TwitterData { ScreenName = "Satoshi", TwitterId = 12345 });
            var twitterProfile = profileItem as TwitterProfile;
            twitterProfile.TwitterId.Should().Be(123);
            twitterProfile.Validated.Should().BeTrue();
            success.Should().BeTrue();
        }

        [Fact]
        public async Task Canceling_Twitter_Profile_Within_2_Minutes_Shoud_Succeed()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<ITwitterInfoProvider>();
            var unicityChecker = A.Fake<IUnicityConstraintsChecker>();
            var user = new ApiUser { Username = "Satoshi" };

            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Twitter)).Returns(Task.FromResult(UnicityResult.True));
            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Profile)).Returns(Task.FromResult(UnicityResult.True));

            var usersService = new UsersServiceBuilder()
                .WithUnicityConstraintsChecker(unicityChecker)
                .WithContextProvider(contextProvider)
                .Build();
            await usersService.SaveProfile(user);

            A.CallTo(() => profileProvider.CheckTweetAndGetTwitterProfile(123, "Satoshi", 12345)).Returns(Task.FromResult(BuildProfile()));

            var twitterService = new TwitterServiceBuilder()
                .WithUsersService(usersService)
                .WithTwitterInfoProvider(profileProvider)
                .Build();

            var (profileItem, success) = await twitterService.SetProfileItem(user, 123, new TwitterData { ScreenName = "Satoshi", TwitterId = 12345 });
            var twitterProfile = profileItem as TwitterProfile;
            twitterProfile.TwitterId.Should().Be(123);
            success.Should().BeTrue();
            SystemTime.SetDateTime(DateTime.Now.AddMinutes(1));
            (profileItem, success) = await twitterService.Cancel(user);
            success.Should().BeTrue();
            profileItem.Should().BeNull();
        }

        [Fact]
        public async Task Canceling_Twitter_Profile_After_More_Than_2_Minutes_Shoud_Fail()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<ITwitterInfoProvider>();
            var unicityChecker = A.Fake<IUnicityConstraintsChecker>();
            var user = new ApiUser { Username = "Satoshi" };

            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Twitter)).Returns(Task.FromResult(UnicityResult.True));
            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Profile)).Returns(Task.FromResult(UnicityResult.True));

            var usersService = new UsersServiceBuilder()
                .WithUnicityConstraintsChecker(unicityChecker)
                .WithContextProvider(contextProvider)
                .Build();
            await usersService.SaveProfile(user);

            A.CallTo(() => profileProvider.CheckTweetAndGetTwitterProfile(123, "Satoshi", 12345)).Returns(Task.FromResult(BuildProfile()));

            var twitterService = new TwitterServiceBuilder()
                .WithUsersService(usersService)
                .WithTwitterInfoProvider(profileProvider)
                .Build();

            var (profileItem, success) = await twitterService.SetProfileItem(user, 123, new TwitterData { ScreenName = "Satoshi", TwitterId = 12345 });
            var twitterProfile = profileItem as TwitterProfile;
            twitterProfile.TwitterId.Should().Be(123);
            success.Should().BeTrue();
            SystemTime.SetDateTime(DateTime.Now.AddMinutes(3));
            (profileItem, success) = await twitterService.Cancel(user);
            twitterProfile = profileItem as TwitterProfile;
            success.Should().BeFalse();
            profileItem.Should().NotBeNull();
            twitterProfile.TwitterId.Should().Be(123);
        }

        [Fact]
        public async Task Updating_A_Validated_Twitter_Profile_Shoud_Succeed()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<ITwitterInfoProvider>();
            var unicityChecker = A.Fake<IUnicityConstraintsChecker>();
            var user = new ApiUser { Username = "Satoshi" };

            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Twitter)).Returns(Task.FromResult(UnicityResult.True));
            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Profile)).Returns(Task.FromResult(UnicityResult.True));

            var usersService = new UsersServiceBuilder()
                .WithUnicityConstraintsChecker(unicityChecker)
                .WithContextProvider(contextProvider)
                .Build();
            await usersService.SaveProfile(user);

            A.CallTo(() => profileProvider.CheckTweetAndGetTwitterProfile(123, "Satoshi", 12345)).Returns(Task.FromResult(BuildProfile()));

            var twitterService = new TwitterServiceBuilder()
                .WithUsersService(usersService)
                .WithTwitterInfoProvider(profileProvider)
                .Build();

            var (profileItem, success) = await twitterService.SetProfileItem(user, 123, new TwitterData { ScreenName = "Satoshi", TwitterId = 12345 });
            var twitterProfile = profileItem as TwitterProfile;
            twitterProfile.TwitterId.Should().Be(123);
            twitterProfile.Username.Should().Be("Satoshi");
            twitterProfile.Validated.Should().BeTrue();
            success.Should().BeTrue();

            A.CallTo(() => profileProvider.GetTwitterProfile(123)).Returns(Task.FromResult(BuildUpdatedProfile()));

            profileItem = await twitterService.Update(user, 123);
            twitterProfile = profileItem as TwitterProfile;
            twitterProfile.TwitterId.Should().Be(123);
            twitterProfile.NumberOfFollower.Should().Be(20);
        }
    }
}