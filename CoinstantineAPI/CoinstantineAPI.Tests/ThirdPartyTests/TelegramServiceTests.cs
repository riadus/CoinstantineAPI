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
using FluentAssertions.Specialized;
using Xunit;

namespace CoinstantineAPI.Tests.ThirdPartyTests
{
    public class TelegramServiceTests
    {
        private (TelegramProfile, string) BuildProfile()
        {
            return (new TelegramProfile
            {
                TelegramId = 123,
                Username = "Satoshi",
                FirstName = "Satoshi"
            }, "123456789");
        }

        [Fact]
        public async Task Get_Telegram_User_Should_Return_Telegram_Public_Profile()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<ITelegramInfoProvider>();

            A.CallTo(() => profileProvider.GetTelegramProfile("Satoshi", false)).Returns(BuildProfile());

            var telegramService = new TelegramServiceBuilder()
                .WithContextProvider(contextProvider)
                .WithTelegramInfoProvider(profileProvider)
                .Build();

            var profile = await telegramService.GetProfileItem("Satoshi") as TelegramProfile;
            profile.Username.Should().Be("Satoshi");
            profile.Validated.Should().BeFalse();
        }

        [Fact]
        public async Task Get_Telegram_User_With_Unkown_Id_Should_Return_Null()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<ITelegramInfoProvider>();

            A.CallTo(() => profileProvider.GetTelegramProfile("Satoshi", false)).Returns((null, null));

            var telegramService = new TelegramServiceBuilder()
                .WithTelegramInfoProvider(profileProvider)
                .WithContextProvider(contextProvider)
                .Build();

            var profile = await telegramService.GetProfileItem("Satoshi") as TelegramProfile;
            profile.Should().BeNull();
        }

        [Fact]
        public async Task Set_Telegram_User_Shoud_Save()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<ITelegramInfoProvider>();
            var usersService = A.Fake<IUsersService>();
            var user = new ApiUser();
            A.CallTo(() => profileProvider.GetTelegramProfile("Satoshi", true)).Returns(BuildProfile());

            var telegramService = new TelegramServiceBuilder()
                .WithUsersService(usersService)
                .WithTelegramInfoProvider(profileProvider)
                .WithContextProvider(contextProvider)
                .Build();

            var (profileItem, success) = await telegramService.SetProfileItem(user, "Satoshi");
            var telegramProfile = profileItem as TelegramProfile;
            telegramProfile.Username.Should().Be("Satoshi");
            profileItem.Validated.Should().BeTrue();
            A.CallTo(() => usersService.SetTelegramProfile(user)).MustHaveHappened();
        }

        [Fact]
        public async Task Set_Telegram_User_Shoud_Succeed()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<ITelegramInfoProvider>();
            var unicityChecker = A.Fake<IUnicityConstraintsChecker>();
            var user = new ApiUser { Username = "Satoshi" };

            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Telegram)).Returns(Task.FromResult(UnicityResult.True));
            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Profile)).Returns(Task.FromResult(UnicityResult.True));

            var usersService = new UsersServiceBuilder()
                .WithUnicityConstraintsChecker(unicityChecker)
                .WithContextProvider(contextProvider)
                .Build();
            await usersService.SaveProfile(user);

            A.CallTo(() => profileProvider.GetTelegramProfile("Satoshi", true)).Returns(BuildProfile());

            var telegramService = new TelegramServiceBuilder()
                .WithUsersService(usersService)
                .WithTelegramInfoProvider(profileProvider)
                .WithContextProvider(contextProvider)
                .Build();

            var (profileItem, success) = await telegramService.SetProfileItem(user, "Satoshi");
            var telegramProfile = profileItem as TelegramProfile;
            telegramProfile.Username.Should().Be("Satoshi");
            telegramProfile.Validated.Should().BeTrue();
            success.Should().BeTrue();
        }

        [Fact]
        public async Task Canceling_Telegram_Profile_Within_2_Minutes_Shoud_Succeed()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<ITelegramInfoProvider>();
            var usersService = A.Fake<IUsersService>();
                                
            var user = new ApiUser { Username = "Satoshi", Email="satoshi@domain.io" };
            A.CallTo(() => profileProvider.GetTelegramProfile("Satoshi", true)).Returns(BuildProfile());
            A.CallTo(() => usersService.SetTelegramProfile(user)).Returns(Task.FromResult(new UnicityResult
            {
                AllGood = true
            }));
            A.CallTo(() => usersService.RemoveTelegramProfile(user)).Returns(Task.FromResult(true));

            var telegramService = new TelegramServiceBuilder()
                .WithUsersService(usersService)
                .WithTelegramInfoProvider(profileProvider)
                .Build();

            var (profileItem, success) = await telegramService.SetProfileItem(user, "Satoshi");
            var telegramProfile = profileItem as TelegramProfile;
            telegramProfile.Username.Should().Be("Satoshi");
            success.Should().BeTrue();
            SystemTime.SetDateTime(DateTime.Now.AddMinutes(1));
            (profileItem, success) = await telegramService.Cancel(user);
            success.Should().BeTrue();
            profileItem.Should().BeNull();
        }

        [Fact]
        public async Task Canceling_Telegram_Profile_After_More_Than_2_Minutes_Shoud_Fail()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<ITelegramInfoProvider>();
            var unicityChecker = A.Fake<IUnicityConstraintsChecker>();
            var user = new ApiUser { Username = "Satoshi" };

            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Telegram)).Returns(Task.FromResult(UnicityResult.True));
            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Profile)).Returns(Task.FromResult(UnicityResult.True));


            var usersService = new UsersServiceBuilder()
                .WithUnicityConstraintsChecker(unicityChecker)
                .WithContextProvider(contextProvider)
                .Build();
            await usersService.SaveProfile(user);

            A.CallTo(() => profileProvider.GetTelegramProfile("Satoshi", true)).Returns(BuildProfile());

            var telegramService = new TelegramServiceBuilder()
                .WithUsersService(usersService)
                .WithTelegramInfoProvider(profileProvider)
                .WithContextProvider(contextProvider)
                .Build();

            var (profileItem, success) = await telegramService.SetProfileItem(user, "Satoshi");
            var telegramProfile = profileItem as TelegramProfile;
            telegramProfile.Username.Should().Be("Satoshi");
            success.Should().BeTrue();
            SystemTime.SetDateTime(DateTime.Now.AddMinutes(3));
            (profileItem, success) = await telegramService.Cancel(user);
            telegramProfile = profileItem as TelegramProfile;
            success.Should().BeFalse();
            profileItem.Should().NotBeNull();
            telegramProfile.Username.Should().Be("Satoshi");
        }

        [Fact]
        public async Task Updating_A_Validated_Telegram_Profile_Shoud_Throw_Exception()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var profileProvider = A.Fake<ITelegramInfoProvider>();
            var unicityChecker = A.Fake<IUnicityConstraintsChecker>();
            var user = new ApiUser { Username = "Satoshi" };

            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Telegram)).Returns(Task.FromResult(UnicityResult.True));
            A.CallTo(() => unicityChecker.CheckUnicity(user, UnicityTopic.Profile)).Returns(Task.FromResult(UnicityResult.True));

            var usersService = new UsersServiceBuilder()
                .WithUnicityConstraintsChecker(unicityChecker)
                .WithContextProvider(contextProvider)
                .Build();
            await usersService.SaveProfile(user);

            A.CallTo(() => profileProvider.GetTelegramProfile("Satoshi", true)).Returns(BuildProfile());

            var telegramService = new TelegramServiceBuilder()
                .WithUsersService(usersService)
                .WithTelegramInfoProvider(profileProvider)
                .WithContextProvider(contextProvider)
                .Build();

            var (profileItem, success) = await telegramService.SetProfileItem(user, "Satoshi");
            var telegramProfile = profileItem as TelegramProfile;
            telegramProfile.Username.Should().Be("Satoshi");
            telegramProfile.Validated.Should().BeTrue();
            success.Should().BeTrue();

            Func<Task<IProfileItem>> throws = async () => await telegramService.Update(user, "Satoshi");
            throws.Should().ThrowExactly<NotSupportedException>();
        }
    }
}
