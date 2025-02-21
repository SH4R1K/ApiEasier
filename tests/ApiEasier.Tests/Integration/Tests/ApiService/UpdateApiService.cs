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
                Entities = []
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
      

            var deleteResponse = await _client.DeleteAsync($"/api/ApiService/{newApiService.Name}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateApiService_WithNonExistingName_ReturnsNotFound()
        {
            var nonExistingApiService = new ApiServiceDto
            {
                Name = "TestApiService",
                IsActive = true,
                Description = "TestDescription",
                Entities = []
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/ApiService/{nonExistingApiService.Name}", nonExistingApiService);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateApiService_WithInValidData_ReturnsBadRequest()
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

            newApiService.Name = "Invalid_%$!?._Name";

            var updateResponse = await _client.PutAsJsonAsync($"/api/ApiService/TestApiService", newApiService);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateApiService_WithMissingRequiredFields_ReturnsBadRequest()
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


            // Entities not required, just cant use
            var MissingRequiredFiledsApiService = new 
            {
                IsActive = true,
                Description = "TestDescription",
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/ApiService/TestApiService", MissingRequiredFiledsApiService);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
