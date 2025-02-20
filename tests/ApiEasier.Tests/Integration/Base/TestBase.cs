using ApiEasier.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Mongo2Go;
using Xunit.Abstractions;

namespace ApiEasier.Tests.Integration.Base
{
    public class TestBase : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        protected readonly HttpClient _client;
        protected readonly ITestOutputHelper _output;

        public TestBase(CustomWebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _client = factory.CreateClient();
            _output = output;
        }
    }
}
