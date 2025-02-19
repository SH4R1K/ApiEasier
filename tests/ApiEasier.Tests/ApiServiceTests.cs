using ApiEasier.Api;
using ApiEasier.Bll.Dto;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using Xunit.Abstractions;

namespace ApiEasier.Tests
{
    public class ApiServiceTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public ApiServiceTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _client = factory.CreateClient();
            _output = output;
        }

        [Fact]
        public async Task GetAllApiServices()
        {
            var requestUrl = "/api/ApiService";
            
            var response = await _client.GetAsync(requestUrl);
            var responseBody = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Response: {responseBody}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }
    }
}