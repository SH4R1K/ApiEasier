using ApiEasier.Bll.Dto;
using ApiEasier.Dm.Models.JsonShema;
using ApiEasier.Dm.Models;
using ApiEasier.Tests.Integration.Base;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ApiEasier.Tests.Integration.Tests.ApiEmu
{
    public class DeleteApiEmu(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        public async Task DeletetApiEmu_WithExistData_ReturnsOk()
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

            var deleteResponse1 = await _client.DeleteAsync($"/api/ApiEmu/TestApiService/TestApiEntity/delete/{id}");
            deleteResponse1.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var responseBody = await deleteResponse1.Content.ReadAsStringAsync();
            _output.WriteLine($"Response: {responseBody}");
        }

        [Fact]
        public async Task DeletetApiEmu_WithInvalidId_ReturnsBadRequest()
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

            var id = $"HelloImInvalidId";

            var deleteResponse1 = await _client.DeleteAsync($"/api/ApiEmu/TestApiService/TestApiEntity/delete/{id}");
            deleteResponse1.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseBody = await deleteResponse1.Content.ReadAsStringAsync();
            _output.WriteLine($"Response: {responseBody}");
        }
        [Fact]
        public async Task DeletetApiEmu_WithNonExistData_ReturnsNotFound()
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

            var id = "67bd68363941c44696e5fae6";

            var deleteResponse1 = await _client.DeleteAsync($"/api/ApiEmu/TestApiService/TestApiEntity/delete/{id}");
            deleteResponse1.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var responseBody = await deleteResponse1.Content.ReadAsStringAsync();
            _output.WriteLine($"Response: {responseBody}");
        }
        [Fact]
        public async Task DeleteApiEmu_WhenApiServiceTurnedOffExists_ReturnsNotFound()
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

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var testObject1 = new { name = "Robert", age = 20, email = "robertpaulson@bk.com" };
            var postResponse1 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject1);
            postResponse1.StatusCode.Should().Be(HttpStatusCode.OK);

            var turnOffResponse = await _client.PatchAsync("/api/ApiService/TestApiService/false", null);
            turnOffResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            using var jsonDoc = JsonDocument.Parse(await postResponse1.Content.ReadAsStringAsync());
            var id = $"{jsonDoc.RootElement.GetProperty("_id")}";

            var deleteResponse1 = await _client.DeleteAsync($"/api/ApiEmu/TestApiService/TestApiEntity/delete/{id}");
            deleteResponse1.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteApiEmu_WhenEntityTurnedOffExists_ReturnsNotFound()
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

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var testObject1 = new { name = "Robert", age = 20, email = "robertpaulson@bk.com" };
            var postResponse1 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject1);
            postResponse1.StatusCode.Should().Be(HttpStatusCode.OK);

            var turnOffResponse = await _client.PatchAsync("/api/ApiEntity/TestApiService/TestApiEntity/false", null);
            turnOffResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            using var jsonDoc = JsonDocument.Parse(await postResponse1.Content.ReadAsStringAsync());
            var id = $"{jsonDoc.RootElement.GetProperty("_id")}";

            var deleteResponse1 = await _client.DeleteAsync($"/api/ApiEmu/TestApiService/TestApiEntity/delete/{id}");
            deleteResponse1.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteApiEmu_WhenEndpointTurnedOffExists_ReturnsNotFound()
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

            var deleteResponse1 = await _client.DeleteAsync($"/api/ApiEmu/TestApiService/TestApiEntity/delete/{id}");
            deleteResponse1.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}