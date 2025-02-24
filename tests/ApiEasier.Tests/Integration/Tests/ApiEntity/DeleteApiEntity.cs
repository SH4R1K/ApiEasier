using ApiEasier.Bll.Dto;
using ApiEasier.Tests.Integration.Base;
using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using Xunit.Abstractions;

namespace ApiEasier.Tests.Integration.Tests.ApiEntity
{
    public class DeleteApiEntity(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        public async Task DeleteApiEntity_WithValidName_ReturnsNoContent()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService1",
                IsActive = true,
                Description = "Service 1 description",
                Entities = new List<ApiEntityDto>
                {
                    new ApiEntityDto
                    {
                        Name = "TestApiEntity",
                        IsActive = true,
                        Structure = null
                    }
                }
            };
            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var deleteResponse = await _client.DeleteAsync($"/api/ApiEntity/{newApiService.Name}/{newApiService.Entities[0].Name}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            _output.WriteLine($"Response: {await deleteResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task DeleteApiEntity_WithNonExistingApiServiceName_ReturnsNotFound()
        {
            var deleteResponse = await _client.DeleteAsync($"/api/ApiEntity/NonExistingApiServiceName/ApiEntityName");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await deleteResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task DeleteApiEntity_WithNonExistingName_ReturnsNotFound()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService1",
                IsActive = true,
                Description = "Service 1 description",
                Entities = new List<ApiEntityDto>()
            };
            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var deleteResponse = await _client.DeleteAsync($"/api/ApiEntity/{newApiService.Name}/NonExistingApiEntityName");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await deleteResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task DeleteApiEntity_WithInvalidNameFormat_ReturnsBadRequest()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService1",
                IsActive = true,
                Description = "Service 1 description",
                Entities = new List<ApiEntityDto>()
            };
            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var deleteResponse = await _client.DeleteAsync($"/api/ApiEntity/{newApiService.Name}/Invalid$$#Name");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await deleteResponse.Content.ReadAsStringAsync()}");
        }
    }
}
