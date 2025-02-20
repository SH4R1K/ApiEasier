using ApiEasier.Api;
using ApiEasier.Tests.Integration.Base;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit.Abstractions;

namespace ApiEasier.Tests.Integration
{
    public class ApiServiceTest : TestBase
    {
        public ApiServiceTest(CustomWebApplicationFactory<Program> factory, ITestOutputHelper output) : base(factory, output) { }

        [Fact]
        public async Task GetApiServices_ReturnsOk()
        {
            var requestUrl = "/api/ApiService";

            var response = await _client.GetAsync(requestUrl);
            var responseBody = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Response: {responseBody}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        //[Fact]
        //public async Task GetItemById_ShouldReturnsCorrectApiService()
        //{

        //}
    }
}