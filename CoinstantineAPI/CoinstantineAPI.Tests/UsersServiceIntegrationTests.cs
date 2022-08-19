using System.Threading.Tasks;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using CoinstantineAPI.Tests.Builders;
using CoinstantineAPI.UnitTests;
using Xunit;

namespace CoinstantineAPI.Tests
{
    public class UsersServiceTests
    {
        private const string confirmationCode = "ConfirmationCode";
        private async Task<(IUsersService UsersService, IUserCreationService UserCreationService)> Init()
        {
            var contextProvider = new ContextProviderBuilder().Build();
            var usersService = new UsersServiceBuilder()
                .WithContextProvider(contextProvider).BuildForIntegration();
            var userCreationService = new UserCreationServiceBuilder().WithUsersService(usersService)
                                                                      .WithConfirmationCode(confirmationCode)
                                                                      .WithContextProvider(contextProvider).Build();
            await userCreationService.SubscribeUser(UserBuilder.AccountCreationModels[0]);
            var userIdentity = await userCreationService.GetUserFromConfirmationCode(confirmationCode);
            await userCreationService.CreateApiUser(userIdentity);
            return (usersService, userCreationService);
        }

        private async Task<(IUsersService UsersService, IUserCreationService UserCreationService)> Init(int index)
        {
            var user = UserBuilder.AccountCreationModels[index];
            var contextProvider = new ContextProviderBuilder().Build();
            var usersService = new UsersServiceBuilder()
                .WithContextProvider(contextProvider).BuildForIntegration();
            var userCreationService = new UserCreationServiceBuilder().WithUsersService(usersService)
                                                                      .WithConfirmationCode(confirmationCode)
                                                                      .WithContextProvider(contextProvider).Build();
            await userCreationService.SubscribeUser(user);
            var userIdentity = await userCreationService.GetUserFromConfirmationCode(confirmationCode);
            await userCreationService.CreateApiUser(userIdentity);
            return (usersService, userCreationService);
        }

        private async Task Init(IUserCreationService userCreationService, int index)
        {
            var accountCreationModel = UserBuilder.AccountCreationModels[index];
            await userCreationService.SubscribeUser(accountCreationModel);
            var userIdentity = await userCreationService.GetUserFromConfirmationCode(confirmationCode);
            await userCreationService.CreateApiUser(userIdentity);
        }

        [Fact]
        public async Task Non_Existing_Username_Should_Be_Addable()
        {
            var (usersService, userCreationService) = await Init();
            var accountCorrect = await userCreationService.IsAccountCorrect(new AccountCreationModel
            {
                Email = "email@mail.com",
                Username = "NewUsername",
                Password = "$tr0ngPwd"
            });
            Assert.True(accountCorrect.AllGood);
            Assert.True(accountCorrect.UsernameAvailable);
            Assert.True(accountCorrect.EmailAvailable);
        }

        [Fact]
        public async Task Existing_Username_Should_Not_Be_Addable()
        {
            var apiUser = UserBuilder.AccountCreationModels[0];
            var (usersService, userCreationService) = await Init();
            var accountCorrect = await userCreationService.IsAccountCorrect(new AccountCreationModel
            {
                Email = apiUser.Email,
                Username = apiUser.Username,
                Password = "password"
            });
            Assert.False(accountCorrect.AllGood);
            Assert.False(accountCorrect.UsernameAvailable);
            Assert.False(accountCorrect.EmailAvailable);
        }

        [Fact]
        public async Task Adding_User_Should_Be_Possible()
        {
            var (UsersService, UserCreationService) = await Init(0);
            var user = await UsersService.GetUserFromEmail(UserBuilder.AccountCreationModels[0].Email);
            Assert.NotNull(user);
            Assert.Equal(user.Username, "Moh");
        }

        [Fact]
        public async Task Adding_User_With_Same_Twitter_Username_Should_Fail()
        {
            var (UsersService, UserCreationService) = await Init();
            await UsersService.SetTwitterProfile(new ApiUser { Username = "Moh", TwitterProfile = new TwitterProfile { Username = "@Moh" } });
            await Init(UserCreationService, 1);
            var result = await UsersService.SetTwitterProfile(new ApiUser { Username = "Hadj", TwitterProfile = new TwitterProfile { Username = "@Moh" } });
            Assert.False(result.AllGood);
            Assert.Contains(UniqueKey.Twitter, result.FailedConstaints);
            Assert.DoesNotContain(UniqueKey.Facebook, result.FailedConstaints);
            Assert.DoesNotContain(UniqueKey.DeviceId, result.FailedConstaints);
            AssertExtentions.Count(result.FailedConstaints, 1);
        }

        [Fact]
        public async Task Adding_User_With_Same_Telegram_Username_Should_Fail()
        {
            var (UsersService, UserCreationService) = await Init();
            await UsersService.SetTelegramProfile(new ApiUser { Username = "Moh", Telegram = new TelegramProfile { Username = "@Moh", TelegramId = 123 } });
            await Init(UserCreationService, 1);
            var result = await UsersService.SetTelegramProfile(new ApiUser { Username = "Hadj", Telegram = new TelegramProfile { Username = "@Moh", TelegramId = 123 } });
            Assert.False(result.AllGood);
            Assert.Contains(UniqueKey.Telegram, result.FailedConstaints);
            Assert.DoesNotContain(UniqueKey.DeviceId, result.FailedConstaints);
            AssertExtentions.Count(result.FailedConstaints, 1);
        }

        [Fact]
        public async Task Adding_User_With_Same_Bct_Username_Should_Fail()
        {
            var (UsersService, UserCreationService) = await Init();
            await UsersService.SetBctProfile(new ApiUser { Username = "Moh", BctProfile = new BitcoinTalkProfile { Username = "moh123", BctId = 123 } });
            await Init(UserCreationService, 1);
            var result = await UsersService.SetBctProfile(new ApiUser { Username = "Hadj", BctProfile = new BitcoinTalkProfile { Username = "moh123", BctId = 1234 } });
            Assert.False(result.AllGood);
            Assert.Contains(UniqueKey.BctName, result.FailedConstaints);
            Assert.DoesNotContain(UniqueKey.DeviceId, result.FailedConstaints);
            AssertExtentions.Count(result.FailedConstaints, 1);
        }

        [Fact]
        public async Task Adding_User_With_Same_Bct_Id_Should_Fail()
        {
            var (UsersService, UserCreationService) = await Init();
            await UsersService.SetBctProfile(new ApiUser { Username = "Moh", BctProfile = new BitcoinTalkProfile { Username = "moh123", BctId = 123 } });
            await Init(UserCreationService, 1);
            var result = await UsersService.SetBctProfile(new ApiUser { Username = "Hadj", BctProfile = new BitcoinTalkProfile { Username = "hadj123", BctId = 123 } });
            Assert.False(result.AllGood);
            Assert.Contains(UniqueKey.BctId, result.FailedConstaints);
            Assert.DoesNotContain(UniqueKey.BctName, result.FailedConstaints);
            AssertExtentions.Count(result.FailedConstaints, 1);
        }

        [Fact]
        public async Task Adding_User_With_Same_Bct_Location_Should_Fail()
        {
            var (UsersService, UserCreationService) = await Init();
            await UsersService.SetBctProfile(new ApiUser { Username = "Moh", BctProfile = new BitcoinTalkProfile { Username = "moh123", Location = "0x123", BctId = 123 } });
            await Init(UserCreationService, 1);
            var result = await UsersService.SetBctProfile(new ApiUser { Username = "Hadj", BctProfile = new BitcoinTalkProfile { Username = "hadj123", Location = "0x123", BctId = 1234 } });
            Assert.False(result.AllGood);
            Assert.Contains(UniqueKey.BctLocation, result.FailedConstaints);
            Assert.DoesNotContain(UniqueKey.DeviceId, result.FailedConstaints);
            Assert.DoesNotContain(UniqueKey.BctName, result.FailedConstaints);
            AssertExtentions.Count(result.FailedConstaints, 1);
        }

        [Fact]
        public async Task Adding_User_With_Same_DeviceId_Location_Should_Fail()
        {
            var usersService = new UsersServiceBuilder().BuildForIntegration();
            await usersService.SaveProfile(new ApiUser { Username = "Moh", DeviceId = "Device123" });
            var result = await usersService.SaveProfile(new ApiUser { Username = "Hadj", DeviceId = "Device123" });
            Assert.False(result.AllGood);
            Assert.Contains(UniqueKey.DeviceId, result.FailedConstaints);
            Assert.DoesNotContain(UniqueKey.UniqueId, result.FailedConstaints);
            AssertExtentions.Count(result.FailedConstaints, 1);
        }

        [Fact]
        public async Task Adding_User_With_Same_Phonenumber_Location_Should_Fail()
        {
            var (UsersService, UserCreationService) = await Init();
            await UsersService.SetTelegramProfile(new ApiUser { Username = "Moh", Phonenumber="0123456789", Telegram = new TelegramProfile { Username = "@Moh", TelegramId = 123 } });
            await Init(UserCreationService, 1);
            var result = await UsersService.SetTelegramProfile(new ApiUser { Username = "Hadj", Phonenumber = "0123456789", Telegram = new TelegramProfile { Username = "@Hadj", TelegramId = 1234 } });

            Assert.False(result.AllGood);
            Assert.Contains(UniqueKey.Phonenumber, result.FailedConstaints);
            Assert.DoesNotContain(UniqueKey.Telegram, result.FailedConstaints);
            AssertExtentions.Count(result.FailedConstaints, 1);
        }

        [Fact]
        public async Task Adding_User_With_Same_UniqueId_Location_Should_Fail()
        {
            var usersService = new UsersServiceBuilder().BuildForIntegration();
            await usersService.SaveProfile(new ApiUser { Username = "Moh", UniqueId="123" });
            var result = await usersService.SaveProfile(new ApiUser { Username = "Hadj", UniqueId = "123" });
            Assert.False(result.AllGood);
            Assert.Contains(UniqueKey.UniqueId, result.FailedConstaints);
            Assert.DoesNotContain(UniqueKey.DeviceId, result.FailedConstaints);
            AssertExtentions.Count(result.FailedConstaints, 1);
        }

        [Fact]
        public async Task Adding_Same_User_Twice_Should_Fail()
        {
            var usersService = new UsersServiceBuilder().BuildForIntegration();
            var moh = UserBuilder.ApiUsers[0];
            await usersService.SaveProfile(moh);
            var result = await usersService.SaveProfile(moh);
            Assert.False(result.AllGood);
        }

        [Fact]
        public async Task Adding_Two_Different_Users_Should_Succeed()
        {
            var usersService = new UsersServiceBuilder().BuildForIntegration();
            var moh = UserBuilder.ApiUsers[0];
            var hadj = UserBuilder.ApiUsers[1];
            await usersService.SaveProfile(moh);
            var result = await usersService.SaveProfile(hadj);
            Assert.True(result.AllGood);
            Assert.DoesNotContain(UniqueKey.UniqueId, result.FailedConstaints);
            Assert.DoesNotContain(UniqueKey.BctLocation, result.FailedConstaints);
            Assert.DoesNotContain(UniqueKey.BctName, result.FailedConstaints);
            Assert.DoesNotContain(UniqueKey.DeviceId, result.FailedConstaints);
            Assert.DoesNotContain(UniqueKey.Facebook, result.FailedConstaints);
            Assert.DoesNotContain(UniqueKey.Telegram, result.FailedConstaints);
            Assert.DoesNotContain(UniqueKey.Twitter, result.FailedConstaints);
            AssertExtentions.Count(result.FailedConstaints, 0);
        }
    }
}
