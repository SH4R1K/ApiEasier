using ApiEasier.Bll.Dto;
using ApiEasier.Tests.Integration.Base;
using System.Net.Http.Json;
using System.Net;
using Xunit.Abstractions;
using FluentAssertions;

namespace ApiEasier.Tests.Integration.Tests.ApiEntity
{
    public class CreateApiEntity(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        public async Task CreateApiEntity_WithValidData_ReturnsCreated()
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

            var newApiEntity = new ApiEntityDto
            {
                Name = "TestApiEntity",
                IsActive = true,
                Structure = null
            };

            var response = await _client.PostAsJsonAsync($"/api/ApiEntity/{newApiService.Name}", newApiEntity);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task CreateApiEntity_WithExistingName_ReturnsConflict()
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

            var newApiEntity = new ApiEntityDto
            {
                Name = "TestApiEntity",
                IsActive = true,
                Structure = null
            };

            var response = await _client.PostAsJsonAsync($"/api/ApiEntity/{newApiService.Name}", newApiEntity);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            response = await _client.PostAsJsonAsync($"/api/ApiEntity/{newApiService.Name}", newApiEntity);
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task CreateApiEntity_WithNonExistingApiServiceName_ReturnsConflict()
        {
            var newApiEntity = new ApiEntityDto
            {
                Name = "TestApiEntity",
                IsActive = true,
                Structure = null
            };

            var response = await _client.PostAsJsonAsync($"/api/ApiEntity/NonExistingApiService", newApiEntity);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task CreateApiEntity_WithInvalidNameFormat_ReturnsBadRequest()
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

            var newApiEntity = new ApiEntityDto
            {
                Name = "Invalid_%$_Name",
                IsActive = true,
                Structure = null
            };

            var response = await _client.PostAsJsonAsync($"/api/ApiEntity/{newApiService.Name}", newApiEntity);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task CreateApiService_WithMissingRequiredFields_ReturnsBadRequest()
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

            var newApiEntity = new
            {
                IsActive = true
            };

            var response = await _client.PostAsJsonAsync($"/api/ApiEntity/{newApiService.Name}", newApiEntity);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }
    }
}
