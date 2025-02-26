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
    public class UpdateApiEmu(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        public async Task UpdatetApiEmu_WithValidData_ReturnsOk()
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
                    new() { Route = "put", Type = TypeResponse.Put, IsActive = true },
                    new() { Route = "delete", Type = TypeResponse.Delete, IsActive = true }
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

            var testObject1 = new { name = "Robert", age = 20, email = "robertpaulson@bk.com" };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var postResponse1 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject1);
            postResponse1.StatusCode.Should().Be(HttpStatusCode.OK);

            using var jsonDoc = JsonDocument.Parse(await postResponse1.Content.ReadAsStringAsync());
            var id = $"{jsonDoc.RootElement.GetProperty("_id")}";

            var updatedTestObject = new { name = "Robertina", age = 20, email = "hehe@m.com" };
            var putResponse1 = await _client.PutAsJsonAsync($"/api/ApiEmu/TestApiService/TestApiEntity/put/{id}", updatedTestObject);
            putResponse1.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await putResponse1.Content.ReadAsStringAsync();
            _output.WriteLine($"Response: {responseBody}");
        }

        [Fact]
        public async Task UpdatetApiEmu_WithInvalidData_ReturnsBadRequest()
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
                    new() { Route = "put", Type = TypeResponse.Put, IsActive = true },
                    new() { Route = "delete", Type = TypeResponse.Delete, IsActive = true }
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

            var testObject1 = new { name = "Robert", age = 20, email = "robertpaulson@bk.com" };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var postResponse1 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject1);
            postResponse1.StatusCode.Should().Be(HttpStatusCode.OK);

            using var jsonDoc = JsonDocument.Parse(await postResponse1.Content.ReadAsStringAsync());
            var id = $"{jsonDoc.RootElement.GetProperty("_id")}";

            var updatedTestObject = new { name = "Robertina", age = 15, email = "hehe@m.com", shouldBeError = true };
            var putResponse1 = await _client.PutAsJsonAsync($"/api/ApiEmu/TestApiService/TestApiEntity/put/{id}", updatedTestObject);
            putResponse1.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseBody = await putResponse1.Content.ReadAsStringAsync();
            _output.WriteLine($"Response: {responseBody}");
        }

        [Fact]
        public async Task UpdatetApiEmu_WithoutStructureData_ReturnsOk()
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
                Endpoints = new List<ApiEndpointDto>
                {
                    new() { Route = "get", Type = TypeResponse.Get, IsActive = true },
                    new() { Route = "post", Type = TypeResponse.Post, IsActive = true },
                    new() { Route = "put", Type = TypeResponse.Put, IsActive = true },
                    new() { Route = "delete", Type = TypeResponse.Delete, IsActive = true }
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

            var testObject1 = new { name = "Robert", age = 20, email = "robertpaulson@bk.com" };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var postResponse1 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject1);
            postResponse1.StatusCode.Should().Be(HttpStatusCode.OK);

            using var jsonDoc = JsonDocument.Parse(await postResponse1.Content.ReadAsStringAsync());
            var id = $"{jsonDoc.RootElement.GetProperty("_id")}";

            var updatedTestObject = new { name = "Robertina", age = 15, email = "hehe@m.com", shouldBeError = false };
            var putResponse1 = await _client.PutAsJsonAsync($"/api/ApiEmu/TestApiService/TestApiEntity/put/{id}", updatedTestObject);
            putResponse1.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await putResponse1.Content.ReadAsStringAsync();
            _output.WriteLine($"Response: {responseBody}");
        }

        [Fact]
        public async Task UpdatetApiEmu_WithNonExistingId_ReturnsNotFound()
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
                Endpoints = new List<ApiEndpointDto>
                {
                    new() { Route = "get", Type = TypeResponse.Get, IsActive = true },
                    new() { Route = "post", Type = TypeResponse.Post, IsActive = true },
                    new() { Route = "put", Type = TypeResponse.Put, IsActive = true },
                    new() { Route = "delete", Type = TypeResponse.Delete, IsActive = true }
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

            var testObject1 = new { name = "Robert", age = 20, email = "robertpaulson@bk.com" };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var id = "67bd68363941c44696e5fae6";

            var updatedTestObject = new { name = "Robertina", age = 15, email = "hehe@m.com", shouldBeError = true };
            var putResponse1 = await _client.PutAsJsonAsync($"/api/ApiEmu/TestApiService/TestApiEntity/put/{id}", updatedTestObject);
            putResponse1.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var responseBody = await putResponse1.Content.ReadAsStringAsync();
            _output.WriteLine($"Response: {responseBody}");
        }

        [Fact]
        public async Task UpdateApiEmu_WhenApiServiceTurnedOffExists_ReturnsNotFound()
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
                    new() { Route = "put", Type = TypeResponse.Put, IsActive = true },
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

            var testObject1 = new { name = "Robert", age = 20, email = "robertpaulson@bk.com" };
            var postResponse1 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject1);
            postResponse1.StatusCode.Should().Be(HttpStatusCode.OK);

            var turnOffResponse = await _client.PatchAsync("/api/ApiService/TestApiService/false", null);
            turnOffResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            using var jsonDoc = JsonDocument.Parse(await postResponse1.Content.ReadAsStringAsync());
            var id = $"{jsonDoc.RootElement.GetProperty("_id")}";

            var updatedTestObject = new { name = "Robertina", age = 15, email = "hehe@m.com", shouldBeError = true };
            var putResponse1 = await _client.PutAsJsonAsync($"/api/ApiEmu/TestApiService/TestApiEntity/put/{id}", updatedTestObject);
            putResponse1.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateApiEmu_WhenEntityTurnedOffExists_ReturnsNotFound()
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
                    new() { Route = "put", Type = TypeResponse.Put, IsActive = true },
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

            var testObject1 = new { name = "Robert", age = 20, email = "robertpaulson@bk.com" };
            var postResponse1 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject1);
            postResponse1.StatusCode.Should().Be(HttpStatusCode.OK);

            var turnOffResponse = await _client.PatchAsync("/api/ApiEntity/TestApiService/TestApiEntity/false", null);
            turnOffResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            using var jsonDoc = JsonDocument.Parse(await postResponse1.Content.ReadAsStringAsync());
            var id = $"{jsonDoc.RootElement.GetProperty("_id")}";

            var updatedTestObject = new { name = "Robertina", age = 15, email = "hehe@m.com", shouldBeError = true };
            var putResponse1 = await _client.PutAsJsonAsync($"/api/ApiEmu/TestApiService/TestApiEntity/put/{id}", updatedTestObject);
            putResponse1.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateApiEmu_WhenEndpointTurnedOffExists_ReturnsNotFound()
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

            var testObject1 = new { name = "Robert", age = 20, email = "robertpaulson@bk.com" };
            var postResponse1 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject1);
            postResponse1.StatusCode.Should().Be(HttpStatusCode.OK);

            using var jsonDoc = JsonDocument.Parse(await postResponse1.Content.ReadAsStringAsync());
            var id = $"{jsonDoc.RootElement.GetProperty("_id")}";

            var updatedTestObject = new { name = "Robertina", age = 15, email = "hehe@m.com", shouldBeError = true };
            var putResponse1 = await _client.PutAsJsonAsync($"/api/ApiEmu/TestApiService/TestApiEntity/put/{id}", updatedTestObject);
            putResponse1.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}