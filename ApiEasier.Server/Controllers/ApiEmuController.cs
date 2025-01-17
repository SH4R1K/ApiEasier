using ApiEasier.Server.Interfaces;
using ApiEasier.Server.Models;
using ApiEasier.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

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
        public async Task<IActionResult> Get(string apiName, string entityName, string endpoint, [FromQuery] Dictionary<string, string>? filters)
        {
            var documents = await _dynamicCollectionService.GetDocFromCollectionAsync(entityName);
            if (documents != null)
                return Ok(documents); // Сериализуем результат из dictionary в json
            else
                return NotFound();
        }

        [HttpGet("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> GetById(string apiName, string entityName, string endpoint, string id, [FromQuery] Dictionary<string, string>? filters)
        {
            var result = await _dynamicCollectionService.GetDocByIdFromCollectionAsync(entityName, id);
             if (result != null) 
                return Ok(result);
             else
                return NotFound();
        }

        [HttpPost("{apiName}/{entityName}/{endpoint}")]
        public async Task<IActionResult> Post(string entityName, object json)
        {
            var result = await _dynamicCollectionService.AddDocToCollectionAsync(entityName, json);

            return Ok(result);
        }

        [HttpPut("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> Put(string entityName, object json)
        {
            var result = await _dynamicCollectionService.UpdateDocFromCollectionAsync(entityName, json);

            if (result != null)
                return Ok(result);
            else 
                return NotFound();
        }

        [HttpDelete("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> Delete(string entityName,string id)
        {
            var result = await _dynamicCollectionService.DeleteDocFromCollectionAsync(entityName, id);
            if (result > 0) 
                return NoContent();
            return NotFound();
        }
    }
}
