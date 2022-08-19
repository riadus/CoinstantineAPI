using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.IntegrationTests.Configuration;
using CoinstantineAPI.IntegrationTests.Fixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace CoinstantineAPI.IntegrationTests
{
    [Order(5)]
    public class CurrentAirdropsIntegrationTests : BaseIntegrationTests
    {
        public CurrentAirdropsIntegrationTests(CoinstantineApiTestServer fixture) : base(fixture)
        {
        }


        [Fact, Order(1)]
        public async Task CurrentAirdrop_ShouldBeCreatedFirstTime()
        {
            var context = _fixture.Services.GetRequiredService<IContextProvider>().CoinstantineContext;
            var aidropDefinitions = await context.AirdropDefinitions.ToListAsync();
            aidropDefinitions.Count.ShouldBe(0);

            var getResponse = await _fixture.Client.GetAsync($"api/airdrops/current");
            getResponse.EnsureSuccessStatusCode();

            context = _fixture.Services.GetRequiredService<IContextProvider>().CoinstantineContext;
            aidropDefinitions = await context.AirdropDefinitions.ToListAsync();
            aidropDefinitions.Count.ShouldBe(2);
        }

        [Fact, Order(2)]
        public async Task CurrentAirdrop_Should_Not_Create_Extra()
        {
            var context = _fixture.Services.GetRequiredService<IContextProvider>().CoinstantineContext;
            var airdrops = await context.AirdropDefinitions.ToListAsync();
            airdrops.Count.ShouldBe(2);
            var getResponse = await _fixture.Client.GetAsync($"api/airdrops/current");
            getResponse.EnsureSuccessStatusCode();

            context = _fixture.Services.GetRequiredService<IContextProvider>().CoinstantineContext;
            airdrops = await context.AirdropDefinitions.ToListAsync();
            airdrops.Count.ShouldBe(2);
        }
    }
}
