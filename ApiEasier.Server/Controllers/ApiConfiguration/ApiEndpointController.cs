using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Bll.Services.ApiConfigure;
using Microsoft.AspNetCore.Mvc;

namespace ApiEasier.Api.Controllers.ApiConfiguration
{
    /// <summary>
    /// Контроллер для обработки действий API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEndpointController : ControllerBase
    {
        private readonly IDynamicEndpointConfigurationService _dynamicEndpointConfigurationService;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ApiEndpointController"/>.
        /// </summary>
        /// <param name="configFileApiService">Сервис для работы с конфигурационными файлами API.</param>
        public ApiEndpointController(
            IDynamicEndpointConfigurationService dynamicEndpointConfigurationService)
        {
            _dynamicEndpointConfigurationService = dynamicEndpointConfigurationService;
        }

        // GET api/ApiAction/{apiServiceName}/{entityName}
        [HttpGet("{apiServiceName}/{apiEntityName}")]
        public async Task<IActionResult> GetAllEndpoints(string apiServiceName, string apiEntityName)
        {
            try
            {
                return Ok(await _dynamicEndpointConfigurationService.GetAsync(apiServiceName, apiEntityName));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // GET api/ApiAction/{apiServiceName}/{entityName}/{actionName}
        [HttpGet("{apiServiceName}/{apiEntityName}/{apiEndpointName}")]
        public async Task<IActionResult> GetEndpointByName(string apiServiceName, string apiEntityName, string apiEndpointName)
        {
            try
            {
                var result = await _dynamicEndpointConfigurationService.GetByIdAsync(apiServiceName, apiEntityName, apiEndpointName);
                if (result == null)
                    return NotFound($"Эндпоинт {apiEndpointName} не найден");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // POST api/ApiAction/{apiServiceName}/{entityName}
        [HttpPost("{apiServiceName}/{apiEntityName}")]
        public async Task<IActionResult> CreateEndpoint(string apiServiceName, string apiEntityName, [FromBody] ApiEndpointDto apiEndpoitn)
        {
            try
            {
                var result = await _dynamicEndpointConfigurationService.CreateAsync(apiServiceName, apiEntityName, apiEndpoitn);
                if (!result)
                    return BadRequest("Не удалось создать эндпоинт");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // PUT api/ApiAction/{apiServiceName}/{entityName}/{actionName}
        [HttpPut("{apiServiceName}/{apiEntityName}/{apiEndpointName}")]
        public async Task<IActionResult> Put(string apiServiceName, string apiEntityName, string apiEndpointName, [FromBody] ApiEndpointDto apiEndpointDto)
        {
            try
            {
                var result =  await _dynamicEndpointConfigurationService.UpdateAsync(apiServiceName, apiEntityName, apiEndpointName, apiEndpointDto);
                if (!result)
                    return BadRequest();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // DELETE api/ApiAction/{apiServiceName}/{entityName}/{actionName}
        [HttpDelete("{apiServiceName}/{apiEntityName}/{apiEndpointName}")]
        public async Task<IActionResult> Delete(string apiServiceName, string apiEntityName, string apiEndpointName)
        {
            try
            {
                var result = await _dynamicEndpointConfigurationService.DeleteAsync(apiServiceName, apiEntityName, apiEndpointName);

                if (!result)
                    return NotFound($"эндпоинт не был удален");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        //PATCH api/ApiService/{apiServiceName}/{apiEntityName}/{apiActionName}/{isActive}
        [HttpPatch("{apiServiceName}/{apiEntityName}/{apiEndpointName}/{isActive}")]
        public async Task<IActionResult> ChangeActiveApiEntity(bool isActive, string apiServiceName, string apiEntityName, string apiEndpointName)
        {
            try
            {
                var result = await _dynamicEndpointConfigurationService.ChangeActiveStatusAsync(apiServiceName, apiEntityName, apiEndpointName, isActive);

                if (!result)
                    return NotFound($"статус у эндпоинта не был изменен");

                return NoContent();
            }
            catch (Exception ex)
            {
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
    }
}

