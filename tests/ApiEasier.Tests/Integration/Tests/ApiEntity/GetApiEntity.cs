using ApiEasier.Bll.Dto;
using ApiEasier.Dm.Models.JsonShema;
using ApiEasier.Tests.Integration.Base;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace ApiEasier.Tests.Integration.Tests.ApiEntity
{
    public class GetApiEntity(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        public async Task GetApiEntityList_WhenApiServiceNotExists_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/ApiEntity/NotExistsApiService");
            var responseBody = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Response: {responseBody}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetApiEntityList_WhenNoDataApiServiceExist_ReturnsOk()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService",
                IsActive = true,
                Description = "TestDescription"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var response = await _client.GetAsync("/api/ApiEntity/TestApiService");

            var entities = await response.Content.ReadFromJsonAsync<List<ApiEntitySummaryDto>>();
            entities.Should().BeEmpty();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetApiEntityList_WhenApiServiceWithDataExist_ReturnsOk()
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
                    },
                    new ApiEntityDto
                    {
                        Name = "TestApiEntity2",
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
                    }
                }
            };
            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var response = await _client.GetAsync("/api/ApiEntity/TestApiService");
            var responseBody = await response.Content.ReadAsStringAsync();
            _output.WriteLine($"Response: {responseBody}");

            var apiEntities = await response.Content.ReadFromJsonAsync<List<ApiEntitySummaryDto>>();
            apiEntities.Should().NotBeNull();
            apiEntities.Should().HaveCountGreaterThan(1);

            apiEntities.Should().Contain(e => e.Name == "TestApiEntity" && e.IsActive && e.Structure == null);
            apiEntities.Should().Contain(e => e.Name == "TestApiEntity2" && !e.IsActive && e.Structure != null);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetApiEntity_WithNonExistingApiServiceAndNonExistingApiEntity_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/ApiEntity/NonExistingApiService/NonExistingApiEntity");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetApiEntity_WithExistingApiServiceAndNonExistingApiEntity_ReturnsNotFound()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService",
                IsActive = true,
                Description = "TestDescription"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var response = await _client.GetAsync("/api/ApiEntity/TestApiService/NotExistApiService");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetApiEntity_WithExistingApiServiceAndExistingApiEntity_ReturnsOk()
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

            var response = await _client.GetAsync("/api/ApiEntity/TestApiService/TestApiEntity");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetApiEntity_WithInvalidNameFormat_ReturnsBadRequest()
        {

            var response = await _client.GetAsync("/api/ApiEntity/Invalid_$#@!?._format");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }
    }
}
