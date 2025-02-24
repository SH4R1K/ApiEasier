using ApiEasier.Bll.Dto;
using ApiEasier.Tests.Integration.Base;
using System.Net.Http.Json;
using System.Net;
using Xunit.Abstractions;
using FluentAssertions;
using Namotion.Reflection;

namespace ApiEasier.Tests.Integration.Tests.ApiEndpoint
{
    public class UpdateApiEndpoint(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        public async Task UpdateApiEndpoint_WithValidData_ReturnsOk()
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

            var newApiEndpoint = new ApiEndpointDto
            {
                Route = "post",
                IsActive = true,
                Type = Dm.Models.TypeResponse.Post
            };

            var updateResponse = await _client.PutAsJsonAsync(
                $"/api/ApiEndpoint/{newApiService.Name}/{newApiService.Entities[0].Name}/{newApiService.Entities[0].Endpoints[0].Route}",
                newApiEndpoint
            );
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedApiEntity = await updateResponse.Content.ReadFromJsonAsync<ApiEndpointDto>();

            updatedApiEntity.Should().NotBeNull();
            updatedApiEntity.Route.Should().Be(newApiEndpoint.Route);
            updatedApiEntity.IsActive.Should().BeTrue();
            updatedApiEntity.Type.Should().Be(Dm.Models.TypeResponse.Post);

            _output.WriteLine($"Response: {await updateResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task UpdateApiEndpoint_WithNonExistingName_ReturnsNotFound()
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
                IsActive = false,
                Type = Dm.Models.TypeResponse.Get
            };

            var updateResponse = await _client.PutAsJsonAsync(
                $"/api/ApiEndpoint/{newApiService.Name}/{newApiService.Entities[0].Name}/NonExsitingApiEndpoint",
                newApiEndpoint
            );
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await updateResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task UpdateApiEndpoint_WithMissingRequiredFields_ReturnsBadRequest()
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

            var missingRequiredFiledsApiEndpoint = new
            {
                IsActive = true,
            };

            var updateResponse = await _client.PutAsJsonAsync(
                $"/api/ApiEndpoint/{newApiService.Name}/{newApiService.Entities[0].Name}/{newApiService.Entities[0].Endpoints[0].Route}",
                missingRequiredFiledsApiEndpoint
            );
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await updateResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task UpdateApiEndpoint_WithInValidData_ReturnsBadRequest()
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

            var newApiEndpoint = new ApiEndpointDto
            {
                Route = "invalid_%@_get_$#",
                IsActive = false,
                Type = Dm.Models.TypeResponse.Get
            };

            var updateResponse = await _client.PutAsJsonAsync(
                $"/api/ApiEndpoint/{newApiService.Name}/{newApiService.Entities[0].Name}/{newApiService.Entities[0].Endpoints[0].Route}",
                newApiEndpoint
            );
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await updateResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task UpdateApiEndpoint_WithDuplicateName_ReturnsConflict()
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
                            new ApiEndpointDto { Route = "get", IsActive = false, Type = Dm.Models.TypeResponse.Get },
                            new ApiEndpointDto { Route = "post", IsActive = false, Type = Dm.Models.TypeResponse.Post }
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

            var updateResponse = await _client.PutAsJsonAsync(
                $"/api/ApiEndpoint/{newApiService.Name}/{newApiService.Entities[0].Name}/{newApiService.Entities[0].Endpoints[1].Route}",
                newApiEndpoint
            );

            updateResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);

            _output.WriteLine($"Response: {await updateResponse.Content.ReadAsStringAsync()}");
        }
    }
}
