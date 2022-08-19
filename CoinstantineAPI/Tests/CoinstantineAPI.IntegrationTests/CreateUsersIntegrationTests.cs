using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Data;
using CoinstantineAPI.IntegrationTests.Configuration;
using CoinstantineAPI.IntegrationTests.Fixture;
using CoinstantineAPI.WebApi.Controllers;
using GST.Fake.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using Xunit;

namespace CoinstantineAPI.IntegrationTests
{
    [Order(1)]
    public class PrepareIntegrationTests : BaseIntegrationTests
    {
        public PrepareIntegrationTests(CoinstantineApiTestServer fixture) : base(fixture)
        {

        }

        [Fact, Order(1)]
        public async Task EnsureDatabaseEmpty()
        {
            await _fixture.EnsureDatabaseEmpty();
            var context = _fixture.Services.GetRequiredService<IContextProvider>().CoinstantineContext;
            var users = await context.UserIdentities.ToListAsync();
            users.Count().ShouldBe(0);
        }
    }

    [Order(2)]
    public class CreateUsersIntegrationTests : BaseIntegrationTests
    {
        public CreateUsersIntegrationTests(CoinstantineApiTestServer fixture) : base(fixture)
        {

        }

        private static int _count;

        [Theory, Order(1)]
        [InlineData("Tester", "Testville", "Testistan", "tester@domain.com", "1000XX", "Testland")]
        [InlineData("Jean", "Paris", "France", "jean@paris.fr", "75000", "Ile de France")]
        [InlineData("Giovani", "Rome", "Italy", "giovani@roma.it", "1000", "Lazio")]
        [InlineData("Jan", "Amsterdam", "Netherlands", "jan@amsterdam.nl", "1000XX", "Noord-Holland")]
        [InlineData("Yahia", "Algiers", "Algeria", "yahia@algiers.dz", "16000", "Alger")]
        public async Task Create_Users(string name, string city, string country, string email, string postalCode, string province)
        {
            _count++;
            var expectedUser = new UserIdentity
            {
                City = city,
                Country = country,
                EmailAddress = email,
                PostalCode = postalCode,
                Province = province
            };
            var claims = new List<Claim>
            {
                new Claim("name", name),
                new Claim("city", expectedUser.City),
                new Claim("country", expectedUser.Country),
                new Claim("province", expectedUser.Province),
                new Claim("postalcode", expectedUser.PostalCode),
                new Claim("emails", expectedUser.EmailAddress)
            };

            var userResolverService = _fixture.Services.GetRequiredService<IUserResolverService>();
            Mock.Get(userResolverService)
                .Setup(x => x.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity(claims)));

            var response = await _fixture.Client.GetAsync("api/users");
            response.EnsureSuccessStatusCode();

            var context = _fixture.Services.GetRequiredService<IContextProvider>().CoinstantineContext;
            var users = await context.UserIdentities.ToListAsync();
            users.Count().ShouldBe(_count);

            var user = users.FirstOrDefault(x => x.EmailAddress == email);
            user.ShouldNotBeNull();

            Assert.Equal(expectedUser.City, user.City);
            Assert.Equal(expectedUser.Country, user.Country);
            Assert.Equal(expectedUser.Province, user.Province);
            Assert.Equal(expectedUser.PostalCode, user.PostalCode);
            Assert.Equal(expectedUser.EmailAddress, user.EmailAddress);
        }
    }
}
