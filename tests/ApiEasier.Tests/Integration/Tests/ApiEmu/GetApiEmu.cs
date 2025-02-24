using ApiEasier.Bll.Dto;
using ApiEasier.Dm.Models;
using ApiEasier.Dm.Models.JsonShema;
using ApiEasier.Tests.Integration.Base;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit.Abstractions;

namespace ApiEasier.Tests.Integration.Tests.ApiEmu
{
    public class GetApiEmu(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        public async Task GetApiEmu_WhenNoDataExists_ReturnsOk()
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

            var getApiEmuResponse = await _client.GetAsync($"/api/ApiEmu/TestApiService/TestApiEntity/get");
            getApiEmuResponse.Should().NotBeNull();
            getApiEmuResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetApiEmu_WhenDataExistsWithoutFilter_ReturnsOk()
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
                    AdditionalProperties = false,
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

            var getApiEmuResponse = await _client.GetAsync($"/api/ApiEmu/TestApiService/TestApiEntity/get");
            getApiEmuResponse.Should().NotBeNull();
            getApiEmuResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var testObject = new { name = "Robert", age = 20, email = "robertpaulson@bk.com" };
            var testObject2 = new { name = "Robertina", age = 25, email = "roberto@kfc.com" };

            var postResponse = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            postResponse = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject2);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var response = await _client.GetAsync("/api/ApiEmu/TestApiService/TestApiEntity/get");
            var responseBody = await response.Content.ReadAsStringAsync();
            _output.WriteLine($"Response: {responseBody}");

            using var jsonDoc = JsonDocument.Parse(responseBody);
            var apiEntities = jsonDoc.RootElement.EnumerateArray().ToList();
            apiEntities.Should().NotBeNull();
            apiEntities.Should().HaveCountGreaterThan(1);

            var isMatch = apiEntities.Any(entity =>
            {
                var name = entity.GetProperty("name").GetString();
                var age = entity.GetProperty("age").GetInt32();
                var email = entity.GetProperty("email").GetString();

                return name == testObject.name && age == testObject.age && email == testObject.email;
            });

            isMatch.Should().BeTrue();
        }

        [Fact]
        public async Task GetApiEmu_WhenDataExistsWithFilter_ReturnsOk()
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
                    AdditionalProperties = false,
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

            var getApiEmuResponse = await _client.GetAsync($"/api/ApiEmu/TestApiService/TestApiEntity/get");
            getApiEmuResponse.Should().NotBeNull();
            getApiEmuResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var testObject = new { name = "Robert", age = 20, email = "robertpaulson@bk.com" };
            var testObject2 = new { name = "Robertina", age = 25, email = "roberto@kfc.com" };

            var postResponse = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            postResponse = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject2);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var filters = $"filters={{\"name\":\"{testObject.name}\"}}";
            var response = await _client.GetAsync($"/api/ApiEmu/TestApiService/TestApiEntity/get?{filters}");
            var responseBody = await response.Content.ReadAsStringAsync();
            _output.WriteLine($"Response: {responseBody}");

            using var jsonDoc = JsonDocument.Parse(responseBody);
            var apiEntities = jsonDoc.RootElement.EnumerateArray().ToList();
            apiEntities.Should().NotBeNull();
            apiEntities.Should().HaveCountGreaterThanOrEqualTo(0);

            var isMatch = apiEntities.All(entity =>
            {
                var name = entity.GetProperty("name").GetString();
                return name == testObject.name;
            });

            isMatch.Should().BeTrue();
        }

        [Fact]
        public async Task GetApiEmu_WhenEndpointTurnedOffExists_ReturnsNotFound()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService",
                IsActive = false,
                Description = "TestDescription",
                Entities = new List<ApiEntityDto>
        {
            new ApiEntityDto
            {
                Name = "TestApiEntity",
                IsActive = false,
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
                    new() { Route = "get", Type = TypeResponse.Get, IsActive = false },
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

            var getApiEmuResponse = await _client.GetAsync($"/api/ApiEmu/TestApiService/TestApiEntity/get");
            getApiEmuResponse.Should().NotBeNull();
            getApiEmuResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetApiEmu_WhenEntityTurnedOffExists_ReturnsNotFound()
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
                IsActive = false,
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

            var getApiEmuResponse = await _client.GetAsync($"/api/ApiEmu/TestApiService/TestApiEntity/get");
            getApiEmuResponse.Should().NotBeNull();
            getApiEmuResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetApiEmu_WhenApiServiceTurnedOffExists_ReturnsNotFound()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService",
                IsActive = false,
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

            var getApiEmuResponse = await _client.GetAsync($"/api/ApiEmu/TestApiService/TestApiEntity/get");
            getApiEmuResponse.Should().NotBeNull();
            getApiEmuResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
