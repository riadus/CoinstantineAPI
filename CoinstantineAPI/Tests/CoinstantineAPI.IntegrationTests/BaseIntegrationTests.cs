using System;
using System.Net.Http.Formatting;
using CoinstantineAPI.IntegrationTests.Fixture;
using GST.Fake.Authentication.JwtBearer;
using Xunit;

namespace CoinstantineAPI.IntegrationTests
{
    public abstract class BaseIntegrationTests : IClassFixture<CoinstantineApiTestServer>
    {
        protected BaseIntegrationTests(CoinstantineApiTestServer fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _mediaTypeFormatter = new JsonMediaTypeFormatter();
            fixture.Init(TestServerFixture.DbSource.InMemory);
            _fixture.Client.SetFakeBearerToken("admin");
        }

        protected readonly JsonMediaTypeFormatter _mediaTypeFormatter;
        protected readonly CoinstantineApiTestServer _fixture;
    }
}
