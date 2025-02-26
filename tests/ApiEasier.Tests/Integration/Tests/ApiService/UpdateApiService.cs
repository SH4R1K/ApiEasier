using ApiEasier.Bll.Dto;
using ApiEasier.Tests.Integration.Base;
using System.Net.Http.Json;
using System.Net;
using Xunit.Abstractions;
using FluentAssertions;

namespace ApiEasier.Tests.Integration.Tests.ApiService
{
    public class UpdateApiService(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        public async Task UpdateApiService_WithValidData_ReturnsOk()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService",
                IsActive = true,
                Description = "TestDescription",
                Entities = new List<ApiEntityDto>()
            };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            newApiService.Description = "Updated TestDescription";
            newApiService.IsActive = false;

            var updateResponse = await _client.PutAsJsonAsync($"/api/ApiService/{newApiService.Name}", newApiService);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedApiService = await updateResponse.Content.ReadFromJsonAsync<ApiServiceDto>();

            updatedApiService.Should().NotBeNull();
            updatedApiService.Name.Should().Be(newApiService.Name);
            updatedApiService.Description.Should().NotBe("TestDescription");
            updatedApiService.IsActive.Should().BeFalse();

            _output.WriteLine($"Response: {await updateResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task UpdateApiService_WithNonExistingName_ReturnsNotFound()
        {
            var nonExistingApiService = new ApiServiceDto
            {
                Name = "TestApiService",
                IsActive = true,
                Description = "TestDescription",
                Entities = new List<ApiEntityDto>()
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/ApiService/{nonExistingApiService.Name}", nonExistingApiService);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await updateResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task UpdateApiService_WithInValidData_ReturnsBadRequest()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService",
                IsActive = true,
                Description = "TestDescription",
                Entities = new List<ApiEntityDto>()
            };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            newApiService.Name = "Invalid_%$!?._Name";

            var updateResponse = await _client.PutAsJsonAsync($"/api/ApiService/TestApiService", newApiService);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await updateResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task UpdateApiService_WithMissingRequiredFields_ReturnsBadRequest()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService",
                IsActive = true,
                Description = "TestDescription",
                Entities = new List<ApiEntityDto>()
            };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var MissingRequiredFiledsApiService = new 
            {
                IsActive = true,
                Description = "TestDescription",
                Entities = new List<ApiEntityDto>()
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/ApiService/TestApiService", MissingRequiredFiledsApiService);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await updateResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task UpdateApiService_WithDuplicateName_ReturnsConflict()
        {
            var apiService1 = new ApiServiceDto
            {
                Name = "TestApiService1",
                IsActive = true,
                Description = "TestDescription",
                Entities = new List<ApiEntityDto>()
            };
            var createResponse1 = await _client.PostAsJsonAsync("/api/ApiService", apiService1);
            createResponse1.StatusCode.Should().Be(HttpStatusCode.Created);

            var apiService2 = new ApiServiceDto
            {
                Name = "TestApiService2",
                IsActive = true,
                Description = "TestDescription",
                Entities = new List<ApiEntityDto>()
            };
            var createResponse2 = await _client.PostAsJsonAsync("/api/ApiService", apiService2);
            createResponse2.StatusCode.Should().Be(HttpStatusCode.Created);

            apiService1.Name = "TestApiService2";
            var updateResponse = await _client.PutAsJsonAsync($"/api/ApiService/TestApiService1", apiService1);

            updateResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);

            _output.WriteLine($"Response: {await updateResponse.Content.ReadAsStringAsync()}");
        }
    }
}
