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

        public ApiEmuController(IDynamicCollectionService dynamicCollectionService, JsonService jsonService, LogService logService)
        {
            _dynamicCollectionService = dynamicCollectionService;
            _jsonService = jsonService;
            _logService = logService;
        }

        [HttpGet("{apiName}/{entityName}/{endpoint}")]
        public async Task<IActionResult> Get(string apiName, string entityName, string endpoint, [FromQuery] string? filters)
        {
            var api = await _jsonService.GetApiServiceByNameAsync(apiName);
            if (api == null)
                return NotFound();

            var entity = api.Entities.FirstOrDefault(e => e.Name == entityName);
            if (entity == null)
                return NotFound();
           
            if (!entity.Actions.Any(a => a.IsActive && a.Route == endpoint && a.Type == TypeResponse.Get))
                return NotFound();


            var result = await _dynamicCollectionService.GetDocFromCollectionAsync($"{apiName}_{entityName}", filters);

            if (result != null)
                return Ok(result); // Сериализуем результат из dictionary в json
            else
                return NotFound();
        }

        [HttpGet("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> GetById(string apiName, string entityName, string endpoint, string id, [FromQuery] object? filters)
        {
            Console.WriteLine(filters.ToBsonDocument());
            var api = await _jsonService.GetApiServiceByNameAsync(apiName);
            if (api == null)
                return NotFound();

            var entity = api.Entities.FirstOrDefault(e => e.Name == entityName);
            if (entity == null)
                return NotFound();

            if (!entity.Actions.Any(a => a.IsActive && a.Route == endpoint && a.Type == TypeResponse.GetByIndex))
                return NotFound();

            var result = new Dictionary<string, string>() ; //await _dynamicCollectionService.GetDocByIdFromCollectionAsync($"{apiName}_{entityName}", id, filters);
             if (result != null) 
                return Ok(result);
             else
                return NotFound();
        }

        [HttpPost("{apiName}/{entityName}/{endpoint}")]
        public async Task<IActionResult> Post(string apiName, string entityName, string endpoint, object json)
        {
            var api = await _jsonService.GetApiServiceByNameAsync(apiName);
            if (api == null)
                return NotFound();

            var entity = api.Entities.FirstOrDefault(e => e.Name == entityName);
            if (entity == null)
                return NotFound();

            if (!entity.Actions.Any(a => a.IsActive && a.Route == endpoint && a.Type == TypeResponse.Post))
                return NotFound();

            var result = await _dynamicCollectionService.AddDocToCollectionAsync($"{apiName}_{entityName}", json);

            return Ok(result);
        }

        [HttpPut("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> Put(string apiName, string entityName, string endpoint, object json)
        {
            var api = await _jsonService.GetApiServiceByNameAsync(apiName);
            if (api == null)
                return NotFound();

            var entity = api.Entities.FirstOrDefault(e => e.Name == entityName);
            if (entity == null)
                return NotFound();

            if (!entity.Actions.Any(a => a.IsActive && a.Route == endpoint && a.Type == TypeResponse.Put))
                return NotFound();

            var result = await _dynamicCollectionService.UpdateDocFromCollectionAsync($"{apiName}_{entityName}", json);

            if (result != null)
                return Ok(result);
            else 
                return NotFound();
        }

        [HttpDelete("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> Delete(string apiName, string entityName, string endpoint, string id)
        {
            var api = await _jsonService.GetApiServiceByNameAsync(apiName);
            if (api == null)
                return NotFound();

            var entity = api.Entities.FirstOrDefault(e => e.Name == entityName);
            if (entity == null)
                return NotFound();

            if (!entity.Actions.Any(a => a.IsActive && a.Route == endpoint && a.Type == TypeResponse.Delete))
                return NotFound();

            var result = await _dynamicCollectionService.DeleteDocFromCollectionAsync($"{apiName}_{entityName}", id);
            if (result > 0) 
                return NoContent();
            return NotFound();
        }
    }
}
