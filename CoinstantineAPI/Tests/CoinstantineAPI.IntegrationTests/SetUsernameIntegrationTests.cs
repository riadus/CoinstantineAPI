using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.IntegrationTests.Configuration;
using CoinstantineAPI.IntegrationTests.Fixture;
using CoinstantineAPI.WebApi.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace CoinstantineAPI.IntegrationTests
{
    [Order(3)]
    public class SetUsernameIntegrationTests : BaseIntegrationTests
    {
        public SetUsernameIntegrationTests(CoinstantineApiTestServer fixture) : base(fixture)
        {

        }

        [Theory, Order(1)]
        [InlineData("tester@domain.com", "Tester")]
        [InlineData("jean@paris.fr", "Jean")]
        [InlineData("giovani@roma.it", "Giovani")]
        [InlineData("jan@amsterdam.nl", "Jan")]
        public async Task Set_Username_Succesfully(string email, string username)
        {
            _fixture.MockUserClaims(email);
            var apiUserDto = new ApiUserDTO
            {
                Username = username
            };
            var response = await _fixture.Client.PostAsync("api/users/username", new ObjectContent<ApiUserDTO>(apiUserDto, _mediaTypeFormatter));
            response.EnsureSuccessStatusCode();

            var context = _fixture.Services.GetRequiredService<IContextProvider>().CoinstantineContext;
            var users = await context.UserIdentities.ToListAsync();
            var user = users.FirstOrDefault(x => x.EmailAddress == email);
            user.ShouldNotBeNull();

            user.EmailAddress.ShouldBe(email);
            user.Username.ShouldBe(username);

            var apiUser = await context.ApiUsers.FirstOrDefaultAsync(x => x.Email == email);
            apiUser.ShouldNotBeNull();
            apiUser.Username.ShouldBe(username);
        }

        [Theory, Order(2)]
        [InlineData("yahia@algiers.dz", "Jan")]
        public async Task Set_Username_Unsuccesfully(string email, string username)
        {
            _fixture.MockUserClaims(email);
            var apiUserDto = new ApiUserDTO
            {
                Username = username
            };
            var response = await _fixture.Client.PostAsync("api/users/username", new ObjectContent<ApiUserDTO>(apiUserDto, _mediaTypeFormatter));
            response.IsSuccessStatusCode.ShouldBeFalse();
        }

        [Theory, Order(3)]
        [InlineData("yahia@algiers.dz", "Yahia")]
        public async Task Set_Username_Retry(string email, string username)
        {
            _fixture.MockUserClaims(email);
            var apiUserDto = new ApiUserDTO
            {
                Username = username
            };
            var response = await _fixture.Client.PostAsync("api/users/username", new ObjectContent<ApiUserDTO>(apiUserDto, _mediaTypeFormatter));
            response.EnsureSuccessStatusCode();

            var context = _fixture.Services.GetRequiredService<IContextProvider>().CoinstantineContext;
            var users = await context.UserIdentities.ToListAsync();
            var user = users.FirstOrDefault(x => x.EmailAddress == email);
            user.ShouldNotBeNull();

            user.EmailAddress.ShouldBe(email);
            user.Username.ShouldBe(username);

            var apiUser = await context.ApiUsers.FirstOrDefaultAsync(x => x.Email == email);
            apiUser.ShouldNotBeNull();
            apiUser.Username.ShouldBe(username);
        }
    }

}
