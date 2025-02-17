using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Logger.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiEasier.Api.Controllers.ApiConfiguration
{

    /// <summary>
    /// Позволяет настраивать API-сервисы 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiServiceController : ControllerBase
    {
        private readonly IDynamicApiConfigurationService _dynamicApiConfigurationService;
        private readonly ILoggerService _logger;

        public ApiServiceController(
            IDynamicApiConfigurationService dynamicApiConfigurationService, ILoggerService logger)
        {
            _dynamicApiConfigurationService = dynamicApiConfigurationService;
            _logger = logger;
        }

        /// <summary>
        /// Возвращает список всех API-сервисов без сущностей
        /// </summary>
        [HttpGet]
        [DisableRequestSizeLimit]
        [ProducesResponseType<List<ApiServiceSummaryDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _dynamicApiConfigurationService.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Возвращает API-сервис по имени
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса</param>
        [HttpGet("{apiServiceName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<ApiServiceDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByName(string apiServiceName)
        {
            try
            {
                var result = await _dynamicApiConfigurationService.GetByIdAsync(apiServiceName);

                if (result == null)
                    return NotFound($"API-сервис {apiServiceName} не найден");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Добавляет новый API-сервис
        /// </summary>
        /// <param name="apiServiceDto">Имя API-сервиса</param>
        [HttpPost]
        [DisableRequestSizeLimit]
        [ProducesResponseType<ApiServiceDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] ApiServiceDto apiServiceDto)
        {
            try
            {
                var result = await _dynamicApiConfigurationService.CreateAsync(apiServiceDto);
                
                if (result == null)
                    return Conflict($"API-сервис {apiServiceDto.Name} уже существует");
             
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Изменяет API-сервис
        /// </summary>
        /// <param name="apiServiceName">Имя изменяемого API-сервиса</param>
        /// <param name="apiServiceDto">Новые данные для API-сервиса</param>
        [HttpPut("{apiServiceName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(string apiServiceName, [FromBody] ApiServiceDto apiServiceDto)
        {
            try
            {
                var result = await _dynamicApiConfigurationService.UpdateAsync(apiServiceName, apiServiceDto);
                if (result == null)
                    return NotFound($"API-сервис {apiServiceName} не найден");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // DELETE api/ApiService/{apiServiceName}
        [HttpDelete("{apiServiceName}")]
        public async Task<IActionResult> Delete(string apiServiceName)
        {
            try
            {
                var result = await _dynamicApiConfigurationService.DeleteAsync(apiServiceName);
                if (!result)
                    return NotFound($"Не удалось найти api-сервис {apiServiceName}");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        //PATCH api/ApiService/{apiServiceName}/{isActive}
        [HttpPatch("{apiServiceName}/{isActive}")]
        public async Task<IActionResult> ChangeActiveApiService(bool isActive, string apiServiceName)
        {
            try
            {
                var result = await _dynamicApiConfigurationService.ChangeActiveStatusAsync(apiServiceName, isActive);

                if (!result)
                    return NotFound($"Не удалось сменить статус api-сервиса {apiServiceName}");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
    }
}

