using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.Core.Encryption;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Database;
using CoinstantineAPI.Encryption;
using CoinstantineAPI.IntegrationTests.Mocks;
using CoinstantineAPI.WebApi.Controllers;
using GST.Fake.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CoinstantineAPI.IntegrationTests.Fixture
{
    public class CoinstantineApiTestServer : TestServerFixture
    {
        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            return base.CreateServer(
                     builder
                    .ConfigureTestServices(services =>
                    {
                        services.AddTransient(x => new Mock<INotificationCenter>().Object);
                        services.AddSingleton(new Mock<IUserResolverService>().Object);
                        services.AddAuthentication(options =>
                        {
                            options.DefaultScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                            options.DefaultAuthenticateScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                        }).AddFakeJwtBearer();
                        services.AddSingleton<IBitcoinTalkPublicProfileProvider, MockBitcoinTalkProvider>();
                        services.AddSingleton<ITwitterInfoProvider, MockTwitterProvider>();
                        services.AddSingleton<ITelegramInfoProvider, MockTelegramInfoProvider>();
                        if (UseInMemory == DbSource.InMemory)
                        {
                            services.AddSingleton(x => new ContextProviderBuilder().Build());
                        }
                        services.AddSingleton<IKeyVaultCrypto, MockKeyVaultCrypto>();
                        services.AddSingleton<IPasswordProvider, MockPasswordProvider>();
                    })
            );
        }

        public void MockUserClaims(string email)
        {
            var claims = new List<Claim>
            {
                new Claim("emails", email)
            };

            var userResolverService = Services.GetRequiredService<IUserResolverService>();
            Mock.Get(userResolverService)
                .Setup(x => x.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity(claims)));
        }
    }

    public static class StaticContext
    {
        private static IContextProvider _contextProvider;
        public static IContextProvider GetContext()
        {
            return _contextProvider ?? (_contextProvider = Build());
        }

        private static IContextProvider Build()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<CoinstantineContext>()
                .UseSqlite(connection)
               //.EnableSensitiveDataLogging()
                .Options;

            return new ContextProvider(options);
        }
    }

    public class ContextProviderBuilder
    {
        public IContextProvider Build()
        {
            return StaticContext.GetContext();
        }
    }

    public class MockKeyVaultCrypto : IKeyVaultCrypto
    {
        public Task<string> DecryptAsync(string keyId, string encryptedText)
        {
            return Task.FromResult("clearText");
        }

        public Task<string> EncryptAsync(string keyId, string value)
        {
            return Task.FromResult("encryptedText");
        }

        public Task<string> GetSecret(string keyId)
        {
            return Task.FromResult("secret");
        }
    }

    public class MockPasswordProvider : PasswordProvider
    {
        public MockPasswordProvider(IEncryptorProvider encryptorProvider) : base(encryptorProvider)
        {
        }

        public override Task<string> GeneratePasswordFromBytes(byte[] bytes)
        {
            return Task.FromResult( "clearText");
        }
    }
}