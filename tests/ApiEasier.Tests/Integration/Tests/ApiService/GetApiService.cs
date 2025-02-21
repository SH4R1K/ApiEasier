using ApiEasier.Api;
using ApiEasier.Bll.Dto;
using ApiEasier.Tests.Integration.Base;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace ApiEasier.Tests.Integration.Tests.ApiService
{
    public class GetApiService(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        public async Task GetApiServiceList_WhenNoDataExists_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/ApiService");
            var responseBody = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Response: {responseBody}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetApiServiceList_WhenDataExists_ReturnsOk()
        {
            var services = new List<ApiServiceDto>
            {
                 new ApiServiceDto { Name = "TestApiService1", IsActive = true, Description = "Service 1 description" },
                 new ApiServiceDto { Name = "TestApiService2", IsActive = false, Description = "Service 2 description" }
            };

            foreach (var service in services)
            {
                var createResponse = await _client.PostAsJsonAsync("/api/ApiService", service);
                createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            }

            var response = await _client.GetAsync("/api/ApiService");
            var responseBody = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Response: {responseBody}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var apiServices = await response.Content.ReadFromJsonAsync<List<ApiServiceDto>>();

            apiServices.Should().NotBeNull();
            apiServices.Should().HaveCountGreaterThanOrEqualTo(2);

            apiServices.Should().Contain(s => s.Name == "TestApiService1" && s.IsActive);
            apiServices.Should().Contain(s => s.Name == "TestApiService2" && !s.IsActive);

            foreach (var service in services)
            {
                var deleteResponse = await _client.DeleteAsync($"/api/ApiService/{service.Name}");
                deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }

        [Fact]
        public async Task GetApiService_WithExistingName_ReturnsOk()
        {
            var newApiService = new ApiServiceDto
            {
                Name = "TestApiService",
                IsActive = true,
                Description = "TestDescription"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var response = await _client.GetAsync("/api/ApiService/TestApiService");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            var deleteResponse = await _client.DeleteAsync($"/api/ApiService/{newApiService.Name}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GetApiService_WithNonExistingName_ReturnsNotFound()
        {

            var response = await _client.GetAsync("/api/ApiService/NotExistApiService");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetApiService_WithInvalidNameFormat_ReturnsBadRequest()
        {

            var response = await _client.GetAsync("/api/ApiService/Invalid_$#@!?._format");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
        }
    }
}
