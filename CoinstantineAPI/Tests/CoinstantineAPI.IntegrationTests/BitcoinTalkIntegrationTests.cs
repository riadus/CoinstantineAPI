using System.Net.Http;
using System.Threading.Tasks;
using CoinstantineAPI.IntegrationTests.Configuration;
using CoinstantineAPI.IntegrationTests.Fixture;
using Shouldly;
using Xunit;

namespace CoinstantineAPI.IntegrationTests
{
    [Order(4)]
    public class BitcoinTalkIntegrationTests : BaseIntegrationTests
    {
        public BitcoinTalkIntegrationTests(CoinstantineApiTestServer fixture) : base(fixture)
        {
           
        }

        [Theory, Order(1)]
        [InlineData("tester@domain.com", 1)]
        [InlineData("jean@paris.fr", 10)]
        [InlineData("giovani@roma.it", 50)]
        [InlineData("yahia@algiers.dz", 100)]
        public async Task SetBitcoinTalkProfile_Successful(string email, long id)
        {
            _fixture.MockUserClaims(email);
            var getResponse = await _fixture.Client.GetAsync($"api/bitcoinTalk/{id}");
            getResponse.EnsureSuccessStatusCode();

            var postResponse = await _fixture.Client.PostAsync($"api/bitcoinTalk/{id}", new ObjectContent<object>(null, _mediaTypeFormatter));
            postResponse.EnsureSuccessStatusCode();
        }

        [Theory, Order(2)]
        [InlineData("jan@amsterdam.nl", 1)]
        public async Task SetBitcoinTalkProfile_Unsuccessful(string email, long id)
        {
            _fixture.MockUserClaims(email);
            var getResponse = await _fixture.Client.GetAsync($"api/bitcoinTalk/{id}");
            getResponse.EnsureSuccessStatusCode();

            var postResponse = await _fixture.Client.PostAsync($"api/bitcoinTalk/{id}", new ObjectContent<object>(null, _mediaTypeFormatter));
            postResponse.IsSuccessStatusCode.ShouldBeFalse();
        }

        [Theory, Order(3)]
        [InlineData("jan@amsterdam.nl", 150)]
        public async Task SetBitcoinTalkProfile_Retry(string email, long id)
        {
            _fixture.MockUserClaims(email);
            var getResponse = await _fixture.Client.GetAsync($"api/bitcoinTalk/{id}");
            getResponse.EnsureSuccessStatusCode();

            var postResponse = await _fixture.Client.PostAsync($"api/bitcoinTalk/{id}", new ObjectContent<object>(null, _mediaTypeFormatter));
            postResponse.EnsureSuccessStatusCode();
        }
    }
}
