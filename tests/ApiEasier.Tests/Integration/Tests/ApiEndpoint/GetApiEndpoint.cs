using ApiEasier.Bll.Dto;
using ApiEasier.Dm.Models.JsonShema;
using ApiEasier.Tests.Integration.Base;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace ApiEasier.Tests.Integration.Tests.ApiEndpoint
{
    public class GetApiEndpoint(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        public async Task GetApiEndpointList_WhenApiServiceNotExists_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/ApiEndpoint/NonExistingApiService/ApiEntity");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetApiEndpointList_WhenApiEntityNotExists_ReturnsNotFound()
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

            var response = await _client.GetAsync($"/api/ApiEndpoint/{newApiService.Name}/NonExistingApiEntity");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetApiEndpointList_WhenApiServiceAndApiEntityWithDataExists_ReturnsOk()
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
                        Name = "TestApiEntity2",
                        IsActive = false,
                        Structure = null,
                        Endpoints = new List<ApiEndpointDto>
                        {
                            new ApiEndpointDto {Route = "get", IsActive = true, Type = Dm.Models.TypeResponse.Get },
                            new ApiEndpointDto {Route = "post", IsActive = false, Type = Dm.Models.TypeResponse.Post }
                        }
                    }
                }
            };
            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var response = await _client.GetAsync($"/api/ApiEndpoint/{newApiService.Name}/{newApiService.Entities[0].Name}");

            var apiEndpoints = await response.Content.ReadFromJsonAsync<List<ApiEndpointDto>>();
            apiEndpoints.Should().NotBeNull();
            apiEndpoints.Should().HaveCountGreaterThan(1);

            apiEndpoints.Should().Contain(e => e.Route == "get" && e.IsActive && e.Type == Dm.Models.TypeResponse.Get);
            apiEndpoints.Should().Contain(e => e.Route == "post" && !e.IsActive && e.Type == Dm.Models.TypeResponse.Post);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetApiEndpoint_WithNonExistingApiServiceAndNonExistingApiEntity_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/ApiEndpoint/NonExistingApiService/NonExistingApiEntity");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetApiEndpoint_WithExistingApiServiceAndNonExistingApiEntity_ReturnsNotFound()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService",
                IsActive = true,
                Description = "TestDescription"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var response = await _client.GetAsync($"/api/ApiEndpoint/{newApiService.Name}/NotExistApiService");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetApiEndpoint_WithExistingApiServiceAndExistingApiEntityAndNonExistingApiEndpoint_ReturnsNotFound()
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

            var response = await _client.GetAsync($"/api/ApiEndpoint/{newApiService.Name}/{newApiService.Entities[0].Name}/NonExistingApiEndpoint");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetApiEndpoint_WithAllExists_ReturnsOk()
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
                        Name = "TestApiEntity2",
                        IsActive = false,
                        Structure = null,
                        Endpoints = new List<ApiEndpointDto>
                        {
                            new ApiEndpointDto {Route = "get", IsActive = true, Type = Dm.Models.TypeResponse.Get },
                            new ApiEndpointDto {Route = "post", IsActive = false, Type = Dm.Models.TypeResponse.Post }
                        }
                    }
                }
            };
            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var response = await _client.GetAsync(
                    $"/api/ApiEndpoint/{newApiService.Name}/{newApiService.Entities[0].Name}/{newApiService.Entities[0].Endpoints[0].Route}"
            );
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var apiEndpoint = await response.Content.ReadFromJsonAsync<ApiEndpointDto>();

            apiEndpoint.Should().NotBeNull();
            apiEndpoint.Route.Should().Be(newApiService.Entities[0].Endpoints[0].Route);
            apiEndpoint.IsActive.Should().BeTrue();
            apiEndpoint.Type.Should().Be(newApiService.Entities[0].Endpoints[0].Type);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetApiEndpoint_WithInvalidNameFormat_ReturnsBadRequest()
        {
            var response = await _client.GetAsync("/api/ApiEndpoint/ApiService/ApiEntity/Invalid_$#@!?._format");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }
    }
}
