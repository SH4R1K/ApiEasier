using ApiEasier.Bll.Interfaces.ApiEmu;
using Microsoft.AspNetCore.Mvc;

namespace ApiEasier.Api.Controllers.ApiEmu
{
    /// <summary>
    /// Контроллер для обработки запросов эмуляции API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEmuController : ControllerBase
    {
        private readonly IDynamicResourceService _dynamicResourceService;

        public ApiEmuController(IDynamicResourceService dynamicResourceService)
        {
            _dynamicResourceService = dynamicResourceService;   
        }

        // GET api/ApiEmu/{apiName}/{entityName}/{endpoint}
        [HttpGet("{apiName}/{entityName}/{endpoint}")]
        public async Task<IActionResult> Get(string apiName, string entityName, string endpoint, [FromQuery] string? filters)
        {
            try
            {
                var data = await _dynamicResourceService.GetAsync(apiName, entityName, endpoint, filters);
                
                if (data == null)
                    return NotFound("Не найдены данные");

                return Ok(data.Select(d => d.Data));
            }
            catch (Exception ex)
            {
                // Логирование исключения
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // GET api/ApiEmu/{apiName}/{entityName}/{endpoint}/{id}
        [HttpGet("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> GetById(string apiName, string entityName, string endpoint, string id, [FromQuery] string? filters)
        {
            try
            {
                var result = await _dynamicResourceService.GetByIdAsync(apiName, entityName, endpoint, id, filters);

                if (result == null)
                    return NotFound("Не найдены данные");

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                // Логирование исключения
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // POST api/ApiEmu/{apiName}/{entityName}/{endpoint}
        [HttpPost("{apiName}/{entityName}/{endpoint}")]
        public async Task<IActionResult> Post(string apiName, string entityName, string endpoint, object json)
        {
            try
            {
                var result = await _dynamicResourceService.AddAsync(apiName, entityName, endpoint, json);
                if (result == null)
                    return NotFound("Данные не были добавлены");

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Логирование исключения
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // PUT api/ApiEmu/{apiName}/{entityName}/{endpoint}/{id}
        [HttpPut("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> Put(string apiName, string entityName, string endpoint, string id, object json)
        {
            try
            {
                var result = await _dynamicResourceService.UpdateAsync(apiName, entityName, endpoint, id, json);

                if (result == null)
                    return NotFound($"Не найдены данные");

                return Ok(result);

            }
            catch (Exception ex)
            {
                // Логирование исключения
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // DELETE api/ApiEmu/{apiName}/{entityName}/{endpoint}/{id}
        [HttpDelete("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> Delete(string apiName, string entityName, string endpoint, string id)
        {
            try
            {
                var result = await _dynamicResourceService.Delete(apiName, entityName, endpoint, id);

                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Логирование исключения
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }
    }
}


