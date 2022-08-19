using System.Net.Http;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Validations;
using CoinstantineAPI.IntegrationTests.Configuration;
using CoinstantineAPI.IntegrationTests.Fixture;
using Shouldly;
using Xunit;

namespace CoinstantineAPI.IntegrationTests
{
    [Order(4)]
    public class TwitterIntegrationTests : BaseIntegrationTests
    {
        public TwitterIntegrationTests(CoinstantineApiTestServer fixture) : base(fixture)
        {

        }

        [Theory, Order(1)]
        [InlineData("tester@domain.com", 1)]
        [InlineData("jean@paris.fr", 10)]
        [InlineData("giovani@roma.it", 40)]
        [InlineData("yahia@algiers.dz", 30)]
        public async Task SetTwitterProfile_Successful(string email, long id)
        {
            _fixture.MockUserClaims(email);
            var getResponse = await _fixture.Client.GetAsync($"api/twitter/{id}");
            getResponse.EnsureSuccessStatusCode();
            var twitterData = new TwitterData
            {
                Username = email,
                TwitterId = id
            };
            var postResponse = await _fixture.Client.PostAsync($"api/twitter/{id}", new ObjectContent<TwitterData>(twitterData, _mediaTypeFormatter));
            postResponse.EnsureSuccessStatusCode();
        }

        [Theory, Order(2)]
        [InlineData("jan@amsterdam.nl", 1)]
        public async Task SetTwitterProfile_Unsuccessful(string email, long id)
        {
            _fixture.MockUserClaims(email);
            var getResponse = await _fixture.Client.GetAsync($"api/twitter/{id}");
            getResponse.EnsureSuccessStatusCode();
            var twitterData = new TwitterData
            {
                Username = email,
                TwitterId = id
            };
            var postResponse = await _fixture.Client.PostAsync($"api/twitter/{id}", new ObjectContent<TwitterData>(twitterData, _mediaTypeFormatter));
            postResponse.IsSuccessStatusCode.ShouldBeFalse();
        }

        [Theory, Order(3)]
        [InlineData("jan@amsterdam.nl", 20)]
        public async Task SetTwitterProfile_Retry(string email, long id)
        {
            _fixture.MockUserClaims(email);
            var getResponse = await _fixture.Client.GetAsync($"api/twitter/{id}");
            getResponse.EnsureSuccessStatusCode();
            var twitterData = new TwitterData
            {
                Username = email,
                TwitterId = id
            };
            var postResponse = await _fixture.Client.PostAsync($"api/twitter/{id}", new ObjectContent<TwitterData>(twitterData, _mediaTypeFormatter));
            postResponse.EnsureSuccessStatusCode();
        }
    }
}
