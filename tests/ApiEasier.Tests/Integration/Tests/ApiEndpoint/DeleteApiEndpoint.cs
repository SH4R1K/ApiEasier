using ApiEasier.Bll.Dto;
using ApiEasier.Tests.Integration.Base;
using System.Net.Http.Json;
using System.Net;
using Xunit.Abstractions;
using FluentAssertions;

namespace ApiEasier.Tests.Integration.Tests.ApiEndpoint
{
    public class DeleteApiEndpoint(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        public async Task DeleteApiEndpoint_WithValidName_ReturnsNoContent()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService",
                IsActive = true,
                Description = "TestDescription",
                Entities = new List<ApiEntityDto>
                {
                    new ApiEntityDto
                    {
                        Name = "TestApiEntity",
                        IsActive = true,
                        Structure = null,
                        Endpoints = new List<ApiEndpointDto>
                        {
                            new ApiEndpointDto { Route = "get", IsActive = false, Type = Dm.Models.TypeResponse.Get }
                        }
                    }
                }
            };
            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var deleteResponse = await _client.DeleteAsync(
                $"/api/ApiEndpoint/{newApiService.Name}/{newApiService.Entities[0].Name}/{newApiService.Entities[0].Endpoints[0].Route}"
            );
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            _output.WriteLine($"Response: {await deleteResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task DeleteApiEndpoint_WithNonExistingApiServiceName_ReturnsNotFound()
        {
            var deleteResponse = await _client.DeleteAsync($"/api/ApiEndpoint/NonExistingApiServiceName/ApiEntityName/ApiEndpoint");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await deleteResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task DeleteApiEndpoint_WithNonExistingApiEntityName_ReturnsNotFound()
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

            var deleteResponse = await _client.DeleteAsync($"/api/ApiEndpoint/{newApiService.Name}/NonExistingApiEntityName/ApiEndpoint");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await deleteResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task DeleteApiEndpoint_WithNonExistingName_ReturnsNotFound()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService",
                IsActive = true,
                Description = "TestDescription",
                Entities = new List<ApiEntityDto>
                {
                    new ApiEntityDto
                    {
                        Name = "TestApiEntity",
                        IsActive = true,
                        Structure = null,
                        Endpoints = new List<ApiEndpointDto>()
                    }
                }
            };
            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var deleteResponse = await _client.DeleteAsync(
                $"/api/ApiEndpoint/{newApiService.Name}/{newApiService.Entities[0].Name}/NonExistingApiEndpoint"
            );
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await deleteResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task DeleteApiEndpoint_WithInvalidNameFormat_ReturnsBadRequest()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService",
                IsActive = true,
                Description = "TestDescription",
                Entities = new List<ApiEntityDto>
                {
                    new ApiEntityDto
                    {
                        Name = "TestApiEntity",
                        IsActive = true,
                        Structure = null,
                        Endpoints = new List<ApiEndpointDto>
                        {
                            new ApiEndpointDto { Route = "get", IsActive = false, Type = Dm.Models.TypeResponse.Get }
                        }
                    }
                }
            };
            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var deleteResponse = await _client.DeleteAsync(
                $"/api/ApiEndpoint/{newApiService.Name}/{newApiService.Entities[0].Name}/Invalid$$#Name"
            );
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await deleteResponse.Content.ReadAsStringAsync()}");
        }
    }
}
