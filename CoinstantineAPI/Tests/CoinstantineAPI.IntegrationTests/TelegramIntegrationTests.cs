using System.Net.Http;
using System.Threading.Tasks;
using CoinstantineAPI.IntegrationTests.Configuration;
using CoinstantineAPI.IntegrationTests.Fixture;
using Shouldly;
using Xunit;

namespace CoinstantineAPI.IntegrationTests
{
    [Order(4)]
    public class TelegramIntegrationTests : BaseIntegrationTests
    {
        public TelegramIntegrationTests(CoinstantineApiTestServer fixture) : base(fixture)
        {

        }

        [Theory, Order(1)]
        [InlineData("tester@domain.com", "tester")]
        [InlineData("jean@paris.fr", "jean")]
        [InlineData("giovani@roma.it", "giovani")]
        [InlineData("yahia@algiers.dz", "yahia")]
        public async Task SetTelegramProfile_Succesful(string email, string id)
        {
            _fixture.MockUserClaims(email);
            var getResponse = await _fixture.Client.GetAsync($"api/telegram/{id}");
            getResponse.EnsureSuccessStatusCode();

            var postResponse = await _fixture.Client.PostAsync($"api/telegram/{id}", new ObjectContent<object>(null, _mediaTypeFormatter));
            postResponse.EnsureSuccessStatusCode();
        }

        [Theory, Order(2)]
        [InlineData("jan@amsterdam.nl", "tester")]
        public async Task SetTelegramProfile_Unsuccessful(string email, string id)
        {
            _fixture.MockUserClaims(email);
            var getResponse = await _fixture.Client.GetAsync($"api/telegram/{id}");
            getResponse.EnsureSuccessStatusCode();

            var postResponse = await _fixture.Client.PostAsync($"api/telegram/{id}", new ObjectContent<object>(null, _mediaTypeFormatter));
            postResponse.IsSuccessStatusCode.ShouldBeFalse();
        }

        [Theory, Order(3)]
        [InlineData("jan@amsterdam.nl", "jan")]
        public async Task SetTelegramProfile_Retry(string email, string id)
        {
            _fixture.MockUserClaims(email);
            var getResponse = await _fixture.Client.GetAsync($"api/telegram/{id}");
            getResponse.EnsureSuccessStatusCode();

            var postResponse = await _fixture.Client.PostAsync($"api/telegram/{id}", new ObjectContent<object>(null, _mediaTypeFormatter));
            postResponse.EnsureSuccessStatusCode();
        }
    }
}
