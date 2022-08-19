using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Security.Claims;
using CoinstantineAPI.Data;
using CoinstantineAPI.IntegrationTests.Fixture;
using CoinstantineAPI.WebApi.Controllers;
using GST.Fake.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CoinstantineAPI.EndToEndTests
{
    public abstract class BaseEndToEndIntegrationTests
    {
        protected BaseEndToEndIntegrationTests()
        {
            _fixture = new CoinstantineApiTestServer();
            _mediaTypeFormatter = new JsonMediaTypeFormatter();
            _fixture.Init(TestServerFixture.DbSource.SQLServer);
            _fixture.Client.SetFakeBearerToken("admin");
        }

        protected readonly JsonMediaTypeFormatter _mediaTypeFormatter;
        protected readonly CoinstantineApiTestServer _fixture;

        public UserIdentity GetUserIdentity(int id)
        {
            return new UserIdentity
            {
                City = "City",
                Country = "Country",
                EmailAddress = $"email{id}@domain.com",
                PostalCode = "75000",
                Province = "Province"
            };
        }

        public void MockClaim(int id)
        {
            var user = GetUserIdentity(id);
            var claims = new List<Claim>
            {
                new Claim("name", $"name{id}"),
                new Claim("city", user.City),
                new Claim("country", user.Country),
                new Claim("province", user.Province),
                new Claim("postalcode", user.PostalCode),
                new Claim("emails", user.EmailAddress)
            };

            var userResolverService = _fixture.Services.GetRequiredService<IUserResolverService>();
            Mock.Get(userResolverService)
                .Setup(x => x.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity(claims)));
        }
    }
}