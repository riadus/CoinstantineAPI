using System.Net.Http;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Airdrops;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Extensions;
using CoinstantineAPI.Data;
using CoinstantineAPI.IntegrationTests.Configuration;
using CoinstantineAPI.IntegrationTests.Fixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace CoinstantineAPI.IntegrationTests
{
    [Order(6)]
    public class SubscribeToAirdropsIntegrationTests : BaseIntegrationTests
    {
        public SubscribeToAirdropsIntegrationTests(CoinstantineApiTestServer fixture) : base(fixture)
        {
        }

        [Theory, Order(1)]
        [InlineData("giovani@roma.it", "Coinstantine Airdrop #1")]
        [InlineData("yahia@algiers.dz", "Coinstantine Airdrop #1")]
        [InlineData("jan@amsterdam.nl", "Coinstantine Airdrop #1")]
        [InlineData("jan@amsterdam.nl", "Coinstantine Airdrop #2")]
        public async Task SubscribeToAirdrop_Successful(string email, string airdropName)
        {
            var context = _fixture.Services.GetRequiredService<IContextProvider>().CoinstantineContext;
            var airdrop = await context.AirdropDefinitions.FirstAsync(x => x.AirdropName == airdropName);
            _fixture.MockUserClaims(email);

            var postResponse = await _fixture.Client.PostAsync($"api/airdrops/current/{airdrop.Id}", new ObjectContent<object>(null, _mediaTypeFormatter));
            postResponse.EnsureSuccessStatusCode();
            var content = await postResponse.Content.ReadAsStringAsync();
            var result = content.DeserializeTo<AirdropSubscriptionResult>();
            result.Success.ShouldBeTrue();
        }

        [Theory, Order(2)]
        [InlineData("tester@domain.com", "Coinstantine Airdrop #1")]
        [InlineData("tester@domain.com", "Coinstantine Airdrop #2")]
        [InlineData("jean@paris.fr", "Coinstantine Airdrop #1")]
        [InlineData("jean@paris.fr", "Coinstantine Airdrop #2")]
        [InlineData("giovani@roma.it", "Coinstantine Airdrop #2")]
        [InlineData("yahia@algiers.dz", "Coinstantine Airdrop #2")]
        public async Task SubscribeToAirdrop_Unsuccessful(string email, string airdropName)
        {
            var context = _fixture.Services.GetRequiredService<IContextProvider>().CoinstantineContext;
            var airdrop = await context.AirdropDefinitions.FirstAsync(x => x.AirdropName == airdropName);
            _fixture.MockUserClaims(email);

            var postResponse = await _fixture.Client.PostAsync($"api/airdrops/current/{airdrop.Id}", new ObjectContent<object>(null, _mediaTypeFormatter));
            postResponse.EnsureSuccessStatusCode();
            var content = await postResponse.Content.ReadAsStringAsync();
            var result = content.DeserializeTo<AirdropSubscriptionResult>();
            result.Success.ShouldBeFalse();
            result.FailReason.ShouldBe(FailReason.RequirementsNotMet);
        }


        [Theory, Order(3)]
        [InlineData("giovani@roma.it", "Coinstantine Airdrop #1")]
        [InlineData("yahia@algiers.dz", "Coinstantine Airdrop #1")]
        [InlineData("jan@amsterdam.nl", "Coinstantine Airdrop #1")]
        [InlineData("jan@amsterdam.nl", "Coinstantine Airdrop #2")]
        public async Task SubscribeToAirdrop_Unsuccessful_Retry(string email, string airdropName)
        {
            var context = _fixture.Services.GetRequiredService<IContextProvider>().CoinstantineContext;
            var airdrop = await context.AirdropDefinitions.FirstAsync(x => x.AirdropName == airdropName);
            _fixture.MockUserClaims(email);

            var postResponse = await _fixture.Client.PostAsync($"api/airdrops/current/{airdrop.Id}", new ObjectContent<object>(null, _mediaTypeFormatter));
            postResponse.EnsureSuccessStatusCode();
            var content = await postResponse.Content.ReadAsStringAsync();
            var result = content.DeserializeTo<AirdropSubscriptionResult>();
            result.Success.ShouldBeFalse();
            result.FailReason.ShouldBe(FailReason.AlreadySubscribed);
        }

        [Theory, Order(4)]
        [InlineData("tester@domain.com", 0)]
        [InlineData("jean@paris.fr", 0)]
        [InlineData("giovani@roma.it", 1)]
        [InlineData("yahia@algiers.dz", 1)]
        [InlineData("jan@amsterdam.nl", 2)]
        public async Task SubscribeToAirdrop_GetMyAirdrops(string email, int count)
        {
            _fixture.MockUserClaims(email);
            var postResponse = await _fixture.Client.GetAsync($"api/airdrops/current/mine");
            postResponse.EnsureSuccessStatusCode();
            var content = await postResponse.Content.ReadAsStringAsync();
            var result = content.DeserializeTo<UserAirdrops>();
            result?.AirdropIds?.Count.ShouldBe(count);
        }
    }
}
