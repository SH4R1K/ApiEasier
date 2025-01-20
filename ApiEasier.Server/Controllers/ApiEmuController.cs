using ApiEasier.Server.Interfaces;
using ApiEasier.Server.Models;
using ApiEasier.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.Net;

namespace ApiEasier.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEmuController : ControllerBase
    {
        private readonly IDynamicCollectionService _dynamicCollectionService;
        private readonly JsonService _jsonService;
        private readonly LogService _logService;
        private readonly IEmuApiValidationService _apiServiceValidator;

        public ApiEmuController(
            IDynamicCollectionService dynamicCollectionService,
            JsonService jsonService,
            LogService logService,
            IEmuApiValidationService apiServiceValidator)
        {
            _dynamicCollectionService = dynamicCollectionService;
            _jsonService = jsonService;
            _logService = logService;
            _apiServiceValidator = apiServiceValidator;
        }

        [HttpGet("{apiName}/{entityName}/{endpoint}")]
        public async Task<IActionResult> Get(string apiName, string entityName, string endpoint, [FromQuery] string? filters)
        {
            var (isValid, _, _) = await _apiServiceValidator.ValidateApiAsync(apiName, entityName, endpoint, TypeResponse.Get);
            if (!isValid) 
                return NotFound();
                
            var documents = await _dynamicCollectionService.GetDocFromCollectionAsync($"{apiName}_{entityName}");
            if (documents != null)
                return Ok(documents); // Сериализуем результат из dictionary в json
            else
                return NotFound();
        }

        [HttpGet("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> GetById(string apiName, string entityName, string endpoint, string id, [FromQuery] object? filters)
        {
            var (isValid, _, _) = await _apiServiceValidator.ValidateApiAsync(apiName, entityName, endpoint, TypeResponse.GetByIndex);
            if (!isValid)
                return NotFound();

            var result = await _dynamicCollectionService.GetDocByIdFromCollectionAsync($"{apiName}_{entityName}", id);
             if (result != null) 
                return Ok(result);
             else
                return NotFound();
        }

        [HttpPost("{apiName}/{entityName}/{endpoint}")]
        public async Task<IActionResult> Post(string apiName, string entityName, string endpoint, object json)
        {
            var (isValid, _, _) = await _apiServiceValidator.ValidateApiAsync(apiName, entityName, endpoint, TypeResponse.Post);
            if (!isValid)
                return NotFound();

            var result = await _dynamicCollectionService.AddDocToCollectionAsync($"{apiName}_{entityName}", json);

            return Ok(result);
        }

        [HttpPut("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> Put(string apiName, string entityName, string endpoint, string id, object json)
        {
            var (isValid, _, _) = await _apiServiceValidator.ValidateApiAsync(apiName, entityName, endpoint, TypeResponse.Put);
            if (!isValid)
                return NotFound();

            var result = await _dynamicCollectionService.UpdateDocFromCollectionAsync($"{apiName}_{entityName}", id, json);

            if (result != null)
                return Ok(result);
            else 
                return NotFound();
        }

        [HttpDelete("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> Delete(string apiName, string entityName, string endpoint, string id)
        {
            var (isValid, _, _) = await _apiServiceValidator.ValidateApiAsync(apiName, entityName, endpoint, TypeResponse.Delete);
            if (!isValid)
                return NotFound();

            var result = await _dynamicCollectionService.DeleteDocFromCollectionAsync($"{apiName}_{entityName}", id);
            if (result > 0) 
                return NoContent();
            return NotFound();
        }
    }
}
