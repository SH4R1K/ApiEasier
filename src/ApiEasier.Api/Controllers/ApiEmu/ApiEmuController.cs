using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Logger.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiEasier.Api.Controllers.ApiEmu
{
    /// <summary>
    /// Контроллер для обработки запросов эмуляции API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEmuController(IDynamicResourceDataService dynamicResourceDataService, ILoggerService logger) : ControllerBase 
    {
        private readonly IDynamicResourceDataService _dynamicResourceDataService = dynamicResourceDataService;
        private readonly ILoggerService _logger = logger;
        
        /// <summary>
        /// Возвращает все данные, принадлежащие указанной сущности
        /// </summary>
        /// <param name="apiName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="entityName">Имя сущности, имеющей эти данные</param>
        /// <param name="endpoint">Эндпоинт сущности с типом Get</param>
        /// <param name="filters">Фильтры для получения данных</param>
        [HttpGet("{apiName}/{entityName}/{endpoint}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<List<object>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllData(string apiName, string entityName, string endpoint, [FromQuery] string? filters)
        {
            try
            {
                var data = await _dynamicResourceDataService.GetAsync(apiName, entityName, endpoint, filters);
                
                if (data == null)
                    return NotFound("Не найдены данные");

                return Ok(data.Select(d => d.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // GET api/ApiEmu/{apiName}/{entityName}/{endpoint}/{id}
        [HttpGet("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> GetById(string apiName, string entityName, string endpoint, string id)
        {
            try
            {
                var result = await _dynamicResourceDataService.GetByIdAsync(apiName, entityName, endpoint, id);

                if (result == null)
                    return NotFound("Не найдены данные");

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // POST api/ApiEmu/{apiName}/{entityName}/{endpoint}
        [HttpPost("{apiName}/{entityName}/{endpoint}")]
        public async Task<IActionResult> Post(string apiName, string entityName, string endpoint, object json)
        {
            try
            {
                var result = await _dynamicResourceDataService.AddAsync(apiName, entityName, endpoint, json);
                if (result == null)
                    return NotFound("Данные не были добавлены");

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // PUT api/ApiEmu/{apiName}/{entityName}/{endpoint}/{id}
        [HttpPut("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> Put(string apiName, string entityName, string endpoint, string id, object json)
        {
            try
            {
                var result = await _dynamicResourceDataService.UpdateAsync(apiName, entityName, endpoint, id, json);

                if (result == null)
                    return NotFound($"Не найдены данные");

                return Ok(result.Data);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // DELETE api/ApiEmu/{apiName}/{entityName}/{endpoint}/{id}
        [HttpDelete("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> Delete(string apiName, string entityName, string endpoint, string id)
        {
            try
            {
                var result = await _dynamicResourceDataService.Delete(apiName, entityName, endpoint, id);

                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }
    }
}


