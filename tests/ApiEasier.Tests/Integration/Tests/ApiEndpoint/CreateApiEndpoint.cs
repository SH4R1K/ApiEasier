using ApiEasier.Bll.Dto;
using ApiEasier.Tests.Integration.Base;
using System.Net.Http.Json;
using System.Net;
using Xunit.Abstractions;
using FluentAssertions;

namespace ApiEasier.Tests.Integration.Tests.ApiEndpoint
{
    public class CreateApiEndpoint(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        public async Task CreateApiEndpoint_WithValidData_ReturnsCreated()
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
                        Structure = null
                    }
                }
            };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var newApiEndpoint = new ApiEndpointDto
            {
                Route = "get",
                IsActive = true,
                Type = Dm.Models.TypeResponse.Get
            };

            var response = await _client.PostAsJsonAsync($"/api/ApiEndpoint/{newApiService.Name}/{newApiService.Entities[0].Name}", newApiEndpoint);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task CreateApiEndpoint_WithExistingName_ReturnsConflict()
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
                            new ApiEndpointDto { Route = "get", IsActive = true, Type = Dm.Models.TypeResponse.Get }
                        }
                    }
                }
            };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var newApiEndpoint = new ApiEndpointDto
            {
                Route = "get",
                IsActive = true,
                Type = Dm.Models.TypeResponse.Get
            };

            var response = await _client.PostAsJsonAsync($"/api/ApiEndpoint/{newApiService.Name}/{newApiService.Entities[0].Name}", newApiEndpoint);
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task CreateApiEndpoint_WithNonExistingApiServiceName_ReturnsNotFoundt()
        {
            var newApiEndpoint = new ApiEndpointDto
            {
                Route = "get",
                IsActive = true,
                Type = Dm.Models.TypeResponse.Get
            };

            var response = await _client.PostAsJsonAsync($"/api/ApiEndpoint/NonExistingApiService/ApiEntity", newApiEndpoint);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task CreateApiEndpoint_WithNonExistingApiEntityName_ReturnsNotFound()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService",
                IsActive = true,
                Description = "TestDescription"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var newApiEndpoint = new ApiEndpointDto
            {
                Route = "get",
                IsActive = true,
                Type = Dm.Models.TypeResponse.Get
            };

            var response = await _client.PostAsJsonAsync($"/api/ApiEndpoint/{newApiService.Name}/NonExistingApiEntity", newApiEndpoint);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task CreateApiEndpoint_WithInvalidNameFormat_ReturnsBadRequest()
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

            var newApiEndpoint = new ApiEndpointDto
            {
                Route = "g%$e#t",
                IsActive = true,
                Type = Dm.Models.TypeResponse.Get
            };

            var response = await _client.PostAsJsonAsync($"/api/ApiEndpoint/{newApiService.Name}/{newApiService.Entities[0].Name}", newApiEndpoint);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task CreateApiEndpoint_WithMissingRequiredFields_ReturnsBadRequest()
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

            var newApiEndpoint = new 
            {
                IsActive = true,
                Type = Dm.Models.TypeResponse.Get
            };

            var response = await _client.PostAsJsonAsync($"/api/ApiEndpoint/{newApiService.Name}/{newApiService.Entities[0].Name}", newApiEndpoint);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }
    }
}
