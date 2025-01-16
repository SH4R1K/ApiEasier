using ApiEasier.Server.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiEasier.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEmuController : ControllerBase
    {
        private readonly IDynamicCollectionService _dynamicCollectionService;

        public ApiEmuController(IDynamicCollectionService dynamicCollectionService)
        {
            _dynamicCollectionService = dynamicCollectionService;
        }

        [HttpGet("{apiName}/{entityName}/{endpoint}")]
        public IActionResult Get(string apiName, string entityName, string endpoint, [FromQuery] Dictionary<string, string>? filters)
        {
            return Ok($"{apiName} {entityName} {endpoint}");
        }

        [HttpGet("{apiName}/{entityName}/{endpoint}/{id}")]
        public IActionResult GetById(string apiName, string entityName, string endpoint, int id, [FromQuery] Dictionary<string, string>? filters)
        {
            return Ok($"{apiName} {entityName} {endpoint} {id}");
        }

        [HttpPost("{apiName}/{entityName}/{endpoint}")]
        public IActionResult Post(string entityName, object json)
        {
            _dynamicCollectionService.AddToCollectionAsync(entityName, json);

            return Ok($"{json.ToString()}");
        }

        [HttpPut("{apiName}/{entityName}/{endpoint}/{id}")]
        public IActionResult Put(int id, object json)
        {
            return Ok($"{id} {json.ToString()}");
        }

        [HttpDelete("{apiName}/{entityName}/{endpoint}/{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
