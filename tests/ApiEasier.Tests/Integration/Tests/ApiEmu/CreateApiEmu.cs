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
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace ApiEasier.Tests.Integration.Tests.ApiEmu
{
    public class CreateApiEmu(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        public async Task CreatetApiEmu_WithValidData_ReturnsOk()
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

            var testObject1 = new { name = "Robert", age = 20, email = "robertpaulson@bk.com" };
            var testObject2 = new { name = "Robertina", age = 25, email = "roberto@kfc.com" };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var postResponse1 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject1);
            postResponse1.StatusCode.Should().Be(HttpStatusCode.OK);
            _output.WriteLine($"Response: {await postResponse1.Content.ReadAsStringAsync()}");

            var postResponse2 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject2);
            postResponse2.StatusCode.Should().Be(HttpStatusCode.OK);
            _output.WriteLine($"Response: {await postResponse2.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task CreatetApiEmu_WithInvalidData_ReturnsBadRequest()
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

            var testObject1 = new { name = "Robert", age = 15, email = "robertpaulson@bk.com" };
            var testObject2 = new { name = "Robertina", age = 25, email = "roberto@kfc.com", shouldbeerror = true };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var postResponse1 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject1);
            postResponse1.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            _output.WriteLine($"Response: {await postResponse1.Content.ReadAsStringAsync()}");

            var postResponse2 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject2);
            postResponse2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            _output.WriteLine($"Response: {await postResponse2.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task CreatetApiEmu_WithoutStructureData_ReturnsOk()
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

            var testObject1 = new { name = "Robert", age = 15, email = "robertpaulson@bk.com" };
            var testObject2 = new { name = "Robertina", age = 25, email = "roberto@kfc.com", shouldbeerror = false };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var postResponse1 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject1);
            postResponse1.StatusCode.Should().Be(HttpStatusCode.OK);
            _output.WriteLine($"Response: {await postResponse1.Content.ReadAsStringAsync()}");

            var postResponse2 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject2);
            postResponse2.StatusCode.Should().Be(HttpStatusCode.OK);
            _output.WriteLine($"Response: {await postResponse2.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task CreateApiEmu_WhenApiServiceTurnedOffExists_ReturnsNotFound()
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

            var testObject1 = new { name = "Robert", age = 15, email = "robertpaulson@bk.com" };
            var postResponse1 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject1);
            postResponse1.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateApiEmu_WhenEntityTurnedOffExists_ReturnsNotFound()
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

            var testObject1 = new { name = "Robert", age = 15, email = "robertpaulson@bk.com" };
            var postResponse1 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject1);
            postResponse1.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateApiEmu_WhenEndpointTurnedOffExists_ReturnsNotFound()
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
                    new() { Route = "post", Type = TypeResponse.Post, IsActive = false },
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

            var testObject1 = new { name = "Robert", age = 15, email = "robertpaulson@bk.com" };
            var postResponse1 = await _client.PostAsJsonAsync("/api/ApiEmu/TestApiService/TestApiEntity/post", testObject1);
            postResponse1.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
