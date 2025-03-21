﻿using ApiEasier.Bll.Dto;
using ApiEasier.Tests.Integration.Base;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace ApiEasier.Tests.Integration.Tests.ApiService
{
    public class CreateApiService(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        public async Task CreateApiService_WithValidData_ReturnsCreated()
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

            var location = createResponse.Headers.Location?.ToString();
            location.Should().NotBeNull();

            var response = await _client.GetAsync(location);

            var apiService = await response.Content.ReadFromJsonAsync<ApiServiceDto>();

            apiService.Should().NotBeNull();
            apiService.Name.Should().Be("TestApiService");
            apiService.IsActive.Should().BeTrue();
            apiService.Description.Should().Be("TestDescription");
            apiService.Entities.Should().BeEmpty();

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task CreateApiService_WithExistingName_ReturnsConflict()
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

            createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);

            createResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);

            _output.WriteLine($"Response: {await createResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task CreateApiService_WithInvalidNameFormat_ReturnsBadRequest()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "Invalid_$#@!?._format",
                IsActive = true,
                Description = "TestDescription",
                Entities = new List<ApiEntityDto>()
            };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);

            createResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await createResponse.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task CreateApiService_WithMissingRequiredFields_ReturnsBadRequest()
        {
            var newApiService = new
            {
                IsActive = true,
                Description = "TestDescription",
                Entities = new List<ApiEntityDto>()
            };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);

            createResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await createResponse.Content.ReadAsStringAsync()}");
        }
    }
}
