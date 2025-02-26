using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Logger.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

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
        /// Возвращает все или отфильтрованные данные, принадлежащие указанной сущности
        /// </summary>
        /// <param name="apiName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="entityName">Имя сущности, имеющей эти данные</param>
        /// <param name="endpoint">Эндпоинт сущности с типом Get</param>
        /// <param name="filters">Фильтры для фильтрации данных</param>
        [HttpGet("{apiName}/{entityName}/{endpoint}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<List<JsonNode>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllData(string apiName, string entityName, string endpoint, [FromQuery] string? filters)
        {
            try
            {
                var data = await _dynamicResourceDataService.GetAsync(apiName, entityName, endpoint, filters);

                if (data == null)
                    return NotFound("По этом адресу эндпоинт не найден");

                return Ok(data.Select(d => d.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Возвращает данные объекта указанной сущности по идентификатору
        /// </summary>
        /// <param name="apiName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="entityName">Имя сущности, имеющей эти данные</param>
        /// <param name="endpoint">Эндпоинт сущности с типом GetByIndex</param>
        /// <param name="id">Идентификатор данных</param>
        [HttpGet("{apiName}/{entityName}/{endpoint}/{id}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<JsonNode>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDataById(string apiName, string entityName, string endpoint, string id)
        {
            try
            {
                var result = await _dynamicResourceDataService.GetByIdAsync(apiName, entityName, endpoint, id);

                if (result == null)
                    return NotFound("По этом адресу эндпоинт не найден");

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Создает объект указаной сущности 
        /// </summary>
        /// <param name="apiName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="entityName">Имя сущности, имеющей эти данные</param>
        /// <param name="endpoint">Эндпоинт сущности с типом Post</param>
        /// <param name="data">Данные нового объекта</param>
        [HttpPost("{apiName}/{entityName}/{endpoint}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<JsonNode>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddData(string apiName, string entityName, string endpoint, object data)
        {
            try
            {
                var result = await _dynamicResourceDataService.AddAsync(apiName, entityName, endpoint, data);
                if (result == null)
                    return NotFound("По этом адресу эндпоинт не найден");

                return Ok(result.Data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Изменяет объект указанной сущности по идентификатору
        /// </summary>
        /// <param name="apiName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="entityName">Имя сущности, имеющей эти данные</param>
        /// <param name="endpoint">Эндпоинт сущности с типом Put</param>
        /// <param name="id">Идентификатор объекта сущности</param>
        /// <param name="data">Данные нового объекта</param>
        [HttpPut("{apiName}/{entityName}/{endpoint}/{id}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<JsonNode>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateData(string apiName, string entityName, string endpoint, string id, object data)
        {
            try
            {
                var result = await _dynamicResourceDataService.UpdateAsync(apiName, entityName, endpoint, id, data);

                if (result == null)
                    return NotFound($"По этом адресу эндпоинт не найден");

                return Ok(result.Data);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Удаляет объект указанной сущности по идентификатору
        /// </summary>
        /// <param name="apiName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="entityName">Имя сущности, имеющей эти данные</param>
        /// <param name="endpoint">Эндпоинт сущности с типом Delete</param>
        /// <param name="id">Идентификатор объекта сущности</param>
        [HttpDelete("{apiName}/{entityName}/{endpoint}/{id}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteData(string apiName, string entityName, string endpoint, string id)
        {
            try
            {
                var result = await _dynamicResourceDataService.Delete(apiName, entityName, endpoint, id);

                if (!result)
                    return NotFound($"По этом адресу эндпоинт не найден");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
    }
}


