using ApiEasier.Bll.Dto;
using ApiEasier.Tests.Integration.Base;
using System.Net.Http.Json;
using System.Net;
using Xunit.Abstractions;
using FluentAssertions;

namespace ApiEasier.Tests.Integration.Tests.ApiEntity
{
    public class UpdateApiEntity(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        public async Task UpdateApiEntity_WithValidData_ReturnsOk()
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

            var newApiEntity = new ApiEntityDto
            {
                Name = "TestApiEntity",
                IsActive = false,
                Structure = null
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/ApiEntity/{newApiService.Name}/{newApiService.Entities[0].Name}", newApiEntity);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedApiEntity = await updateResponse.Content.ReadFromJsonAsync<ApiEntityDto>();

            updatedApiEntity.Should().NotBeNull();
            updatedApiEntity.Name.Should().Be(newApiEntity.Name);
            updatedApiEntity.IsActive.Should().BeFalse();

            _output.WriteLine($"Response: {await updateResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task UpdateApiEntity_WithNonExistingName_ReturnsNotFound()
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

            var newApiEntity = new ApiEntityDto
            {
                Name = "TestApiEntity",
                IsActive = false,
                Structure = null
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/ApiEntity/{newApiService.Name}/NotExistEntityName", newApiEntity);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await updateResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task UpdateApiEntity_WithMissingRequiredFields_ReturnsBadRequest()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService1",
                IsActive = true,
                Description = "Service 1 description",
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

            var missingRequiredFiledsApiEntity = new
            {
                IsActive = true,
            };

            var updateResponse = await _client.PutAsJsonAsync(
                $"/api/ApiEntity/{newApiService.Name}/{newApiService.Entities[0].Name}", missingRequiredFiledsApiEntity
            );
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await updateResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task UpdateApiEntity_WithInValidData_ReturnsBadRequest()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService1",
                IsActive = true,
                Description = "Service 1 description",
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

            var invalidApiEntity = new ApiEntityDto
            {
                Name = "Invalid_%$!?._Name",
                IsActive= true,
                Structure = null
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/ApiService/TestApiService", invalidApiEntity);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await updateResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task UpdateApiEntity_WithDuplicateName_ReturnsConflict()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService1",
                IsActive = true,
                Description = "Service 1 description",
                Entities = new List<ApiEntityDto>
                {
                    new ApiEntityDto
                    {
                        Name = "TestApiEntity1",
                        IsActive = true,
                        Structure = null
                    },
                    new ApiEntityDto
                    {
                        Name = "TestApiEntity2",
                        IsActive = true,
                        Structure = null
                    }
                }
            };
            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var newApiEntity = new ApiEntityDto
            {
                Name = "TestApiEntity1",
                IsActive = true,
                Structure = null
            };

            var updateResponse = await _client.PutAsJsonAsync(
                $"/api/ApiEntity/{newApiService.Name}/{newApiService.Entities[1].Name}", newApiEntity
            );

            updateResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);

            _output.WriteLine($"Response: {await updateResponse.Content.ReadAsStringAsync()}");
        }
    }
}
