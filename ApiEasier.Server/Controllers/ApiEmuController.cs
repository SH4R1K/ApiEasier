using ApiEasier.Server.Interfaces;
using ApiEasier.Server.Models;
using ApiEasier.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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
        public async Task<IActionResult> Get(string apiName, string entityName, string endpoint, [FromQuery] Dictionary<string, object>? filters)
        {
            var api = await _jsonService.GetApiServiceByNameAsync(apiName);
            if (api == null)
                return NotFound();

            var entity = api.Entities.FirstOrDefault(e => e.Name == entityName);
            if (entity == null)
                return NotFound();
           
            if (entity.Actions.Any(a => a.IsActive && a.Route == endpoint && a.Type == TypeResponse.Get))
                return NotFound();
            

            var documents = await _dynamicCollectionService.GetDocFromCollectionAsync(entityName);
            if (documents != null)
                return Ok(documents); // Сериализуем результат из dictionary в json
            else
                return NotFound();
        }

        [HttpGet("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> GetById(string apiName, string entityName, string endpoint, string id, [FromQuery] Dictionary<string, object>? filters)
        {
            var api = await _jsonService.GetApiServiceByNameAsync(apiName);
            if (api == null)
                return NotFound();

            var entity = api.Entities.FirstOrDefault(e => e.Name == entityName);
            if (entity == null)
                return NotFound();

            if (entity.Actions.Any(a => a.IsActive && a.Route == endpoint && a.Type == TypeResponse.GetByIndex))
                return NotFound();

            var result = await _dynamicCollectionService.GetDocByIdFromCollectionAsync(entityName, id);
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

            var result = await _dynamicCollectionService.AddDocToCollectionAsync(entityName, json);

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

            if (entity.Actions.Any(a => a.IsActive && a.Route == endpoint))
                return NotFound();

            var result = await _dynamicCollectionService.UpdateDocFromCollectionAsync(entityName, json);

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

            if (entity.Actions.Any(a => a.IsActive && a.Route == endpoint))
                return NotFound();

            var result = await _dynamicCollectionService.DeleteDocFromCollectionAsync(entityName, id);
            if (result > 0) 
                return NoContent();
            return NotFound();
        }
    }
}
