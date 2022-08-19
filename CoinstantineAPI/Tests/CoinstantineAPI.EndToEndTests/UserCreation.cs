using System.Net.Http;
using System.Threading.Tasks;
using CoinstantineAPI.WebApi.DTO;
using NUnit.Framework;

namespace CoinstantineAPI.EndToEndTests
{
    public class UserCreation : BaseEndToEndIntegrationTests
    {
        [Test, Order(1)]
        public async Task PrepareTests()
        {
            await _fixture.EnsureDatabaseEmpty();
        }

        private int _count;

        public async Task CreateUsers()
        {
            var id = _count++;
            MockClaim(id);
            var response = await _fixture.Client.GetAsync("api/users");
            response.EnsureSuccessStatusCode();

            var apiUserDto = new ApiUserDTO
            {
                Username = $"username_{id}"
            };
            response = await _fixture.Client.PostAsync("api/users/username", new ObjectContent<ApiUserDTO>(apiUserDto, _mediaTypeFormatter));
            response.EnsureSuccessStatusCode();
        }

        [Test, Repeat(10)]
        public async Task FirstBatch()
        {
            await CreateUsers();
        }

        [Test, Repeat(10)]
        public async Task SecondBatch()
        {
            await CreateUsers();
        }
    }
}