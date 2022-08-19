using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CoinstantineAPI.Core;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.IntegrationTests.Fixture
{
    public abstract class TestServerFixture : IDisposable
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .Build();

        public void Init(DbSource dbSource)
        {
            UseInMemory = dbSource;
            TestServer = CreateServer(new WebHostBuilder());
            SetEnvironmentVariables();
            Client = TestServer.CreateClient();
        }

        public enum DbSource
        {
            SQLServer,
            InMemory
        }

        protected DbSource UseInMemory { get; private set; }

        public TestServer TestServer { get; private set; }

        public HttpClient Client { get; set; }

        public IServiceProvider Services => TestServer.Host.Services;

        protected virtual TestServer CreateServer(IWebHostBuilder builder)
        {
            return new TestServer(
                builder
                    .UseConfiguration(Configuration)
                    .UseStartup<Startup>()
            );
        }

        public virtual async Task EnsureDatabaseEmpty()
        {
            var context = Services.GetRequiredService<IContextProvider>().CoinstantineContext;
            await context.EnsureDatabaseEmpty();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                TestServer?.Dispose();
                Client?.Dispose();
            }
        }

        protected virtual void SetEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable(Constants.ConnectionDbKey, "server=tcp:coinstantine.database.windows.net,1433; Initial Catalog=CoinstantineDBIntegration; Persist Security Info=False; User ID=intergUser; Password=$Integration!!!; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            Environment.SetEnvironmentVariable(Constants.ApiEnvironmentKey, "Integration");
            Environment.SetEnvironmentVariable(Constants.EthereumEnvironmentKey, "ropsten");
            Environment.SetEnvironmentVariable(Constants.EtherscanUrlKey, "https://ropsten.etherscan.io");
            Environment.SetEnvironmentVariable(Constants.JsonKeyKey, "JsonKey");
            Environment.SetEnvironmentVariable(Constants.OwnerPasswordKey, "OwnerPassword");
            Environment.SetEnvironmentVariable(Constants.OwnerPrivateKeyKey, "OwnerPrivateKey");
            Environment.SetEnvironmentVariable(Constants.PassphraseKeyKey, "PassphraseKey");
            Environment.SetEnvironmentVariable(Constants.PhonenumberKeyKey, "PhonenumberKey");
            Environment.SetEnvironmentVariable(Constants.TelegramBotTokenKey, "525929086:AAFbW9nsUAhIrQGuw_xLAbRIxElbpyPJQPQ");
            Environment.SetEnvironmentVariable(Constants.TelegramTimeoutKey, "15");
            Environment.SetEnvironmentVariable(Constants.TelegramUrlWebhookKey, "https://coinstantine-acceptance.azurewebsites.net");
            Environment.SetEnvironmentVariable(Constants.TwitterAccessTokenKey, "1011264838801264640-ClsWlLo9uxYgdDYu10p514d0LYftrX");
            Environment.SetEnvironmentVariable(Constants.TwitterAccessTokenSecretKey, "QQFuZWUoTKqPghRDUDwhMoTtL8Uh7P40SHqPQdyKbc09D");
            Environment.SetEnvironmentVariable(Constants.TwitterKeyKey, "erCtN34A6YbXDi9IYl30FmM0R");
            Environment.SetEnvironmentVariable(Constants.TwitterSecretKey, "ArhhHeQBQxIBwu0FW16Hxt6oSX7y38DpkmoyVaULUhW4UuTKdl");
            Environment.SetEnvironmentVariable(Constants.UserPasswordKey, "UserPassword");
            Environment.SetEnvironmentVariable(Constants.VaultClientCredKey, "VIzhk2YOEtYxDtN5rAETeCLaAy78Nim8lbkfH2I9Bp4=");
            Environment.SetEnvironmentVariable(Constants.VaultClientResourceKey, "cd6b79ce-85a7-460e-8089-ef98547e0575");
            Environment.SetEnvironmentVariable(Constants.VaultUrlKey, "https://coinstantinekeyvault.vault.azure.net:443");
            Environment.SetEnvironmentVariable(Constants.Web3UrlKey, "https://ropsten.infura.io/7TQXvoO1gnJVu97A1sfI");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
