using ApiEasier.Bll.Dto;
using ApiEasier.Tests.Integration.Base;
using System.Net.Http.Json;
using System.Net;
using Xunit.Abstractions;
using FluentAssertions;

namespace ApiEasier.Tests.Integration.Tests.ApiService
{
    public class DeleteApiService(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        public async Task DeleteApiService_WithValidName_ReturnsNoContent()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService",
                IsActive = true,
                Description = "TestDescription",
                Entities = []
            };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);

            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var location = createResponse.Headers.Location?.ToString();
            location.Should().NotBeNull();

            var response = await _client.GetAsync(location);
            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            var apiService = await response.Content.ReadFromJsonAsync<ApiServiceDto>();
            apiService.Should().NotBeNull();

            var deleteResponse = await _client.DeleteAsync($"/api/ApiService/{apiService.Name}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteApiService_WithNonExistingName_ReturnsNotFound()
        {
            var deleteResponse = await _client.DeleteAsync($"/api/ApiService/NonExistingName");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteApiService_WithInvalidNameFormat_ReturnsBadRequest()
        {
            var deleteResponse = await _client.DeleteAsync($"/api/ApiService/Invalid$$#Name");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
