using ApiEasier.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Mongo2Go;
using Xunit.Abstractions;

namespace ApiEasier.Tests.Integration.Base
{
    public class TestBase : IAsyncLifetime
    {
        protected HttpClient _client;
        protected ITestOutputHelper _output;
        private CustomWebApplicationFactory<Program> _factory;

        public TestBase(ITestOutputHelper output)
        {
            _output = output;
        }

        public async Task InitializeAsync()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        public async Task DisposeAsync()
        {
            _factory.Dispose();
        }
    }
}
