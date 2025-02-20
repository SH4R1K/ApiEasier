using ApiEasier.Api;
using ApiEasier.Bll.Dto;
using ApiEasier.Dm.Models;
using ApiEasier.Tests.Integration.Base;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
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
                        Structure = null,
                        Endpoints = new List<ApiEndpointDto>
                        {
                            new ApiEndpointDto { Route = "get", Type = TypeResponse.Get, IsActive = true },
                            new ApiEndpointDto { Route = "post", Type = TypeResponse.Post, IsActive = true }
                        }
                    },
                    new ApiEntityDto
                    {
                        Name = "TestApiEntityFulEndpoints",
                        IsActive = true,
                        Structure = null,
                        Endpoints = new List<ApiEndpointDto>
                        {
                            new ApiEndpointDto { Route = "get", Type = TypeResponse.Get, IsActive = true },
                            new ApiEndpointDto { Route = "post", Type = TypeResponse.Post, IsActive = true },
                            new ApiEndpointDto { Route = "put", Type = TypeResponse.Put, IsActive = true },
                            new ApiEndpointDto { Route = "delete", Type = TypeResponse.Delete, IsActive = true }
                        }
                    },
                }
            };

            var createResponse = await _client.PostAsJsonAsync("/api/ApiService", newApiService);

            var responseBody = await createResponse.Content.ReadAsStringAsync();


            _output.WriteLine($"Response: {responseBody}");

            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}