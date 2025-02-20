using ApiEasier.Api;
using ApiEasier.Bll.Dto;
using ApiEasier.Dm.Models;
using ApiEasier.Dm.Models.JsonShema;
using ApiEasier.Tests.Integration.Base;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace ApiEasier.Tests.Integration.Tests
{
    public class ApiServiceTest : TestBase
    {
        public ApiServiceTest(CustomWebApplicationFactory<Program> factory, ITestOutputHelper output) : base(factory, output) { }

        [Fact]
        public async Task GetApiServicesWithoutData_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/ApiService");
            var responseBody = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Response: {responseBody}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetApiServicesWithData_ReturnsOk()
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
                        Structure = new Dm.Models.JsonShema.JsonSchema
                        {
                            Schema = "http://json-schema.org/draft-07/schema#",
                            Type = "object",
                            Required = [ "name", "age", "email" ],
                            Properties = new Dictionary<string, PropertySchema>
                            {
                                { "name", new PropertySchema { Type = "string" } },
                                { "age", new PropertySchema { Type = "integer", Minimum = 18 } },
                                { "email", new PropertySchema { Type = "string", Format = "email" } }
                            }
                        },
                        Endpoints = new List<ApiEndpointDto>
                        {
                            new() { Route = "get", Type = TypeResponse.Get, IsActive = true },
                            new() { Route = "post", Type = TypeResponse.Post, IsActive = true },
                            new() { Route = "put", Type = TypeResponse.Put, IsActive = false },
                            new() { Route = "delete", Type = TypeResponse.Delete, IsActive = false }
                        }
                    },
                    new ApiEntityDto
                    {
                        Name = "TestEmptyApiEntity",
                        IsActive = false,
                        Structure = null
                    },
                }
            };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var location = createResponse.Headers.Location?.ToString();
            location.Should().NotBeNull();

            var getResponse = await _client.GetAsync(location);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var apiService = await getResponse.Content.ReadFromJsonAsync<ApiServiceDto>();

            apiService.Should().NotBeNull();
            apiService.Name.Should().Be("TestApiService");
            apiService.IsActive.Should().BeTrue();
            apiService.Description.Should().Be("TestDescription");
            apiService.Entities.Should().HaveCount(2);

            apiService.Entities[0].Name.Should().Be("TestApiEntity");
            apiService.Entities[0].IsActive.Should().BeTrue();
            apiService.Entities[0].Endpoints.Should().HaveCount(4);
            apiService.Entities[0].Structure.Should().NotBeNull();
            apiService.Entities[0].Endpoints[0].Route.Should().Be("get");
            apiService.Entities[0].Endpoints[0].Type.Should().Be(TypeResponse.Get);
            apiService.Entities[0].Endpoints[0].IsActive.Should().BeTrue();
            apiService.Entities[0].Endpoints[3].Route.Should().Be("delete");
            apiService.Entities[0].Endpoints[3].Type.Should().Be(TypeResponse.Delete);
            apiService.Entities[0].Endpoints[3].IsActive.Should().BeFalse();

            apiService.Entities[1].Name.Should().Be("TestEmptyApiEntity");
            apiService.Entities[1].IsActive.Should().BeFalse();
            apiService.Entities[1].Structure.Should().BeNull();
            apiService.Entities[1].Endpoints.Should().BeEmpty();

        }
    }
}