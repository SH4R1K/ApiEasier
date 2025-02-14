using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Logger.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ApiEasier.Api.Controllers.ApiConfiguration
{

    /// <summary>
    /// Контроллер для настройки api-сервисов
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
        [ProducesResponseType<List<ApiServiceDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByName(string apiServiceName)
        {
            try
            {
                var result = await _dynamicApiConfigurationService.GetByIdAsync(apiServiceName);

                if (result == null)
                    return NotFound($"api-сервис: {apiServiceName} не найден");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // POST api/ApiService
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApiServiceDto apiServiceDto)
        {
            try
            {
                var result = await _dynamicApiConfigurationService.CreateAsync(apiServiceDto);

                if (result == null)
                    return NotFound("Не удалось создать api-сервис");
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // PUT api/ApiService/{oldName}
        [HttpPut("{apiServiceName}")]
        public async Task<IActionResult> Put(string apiServiceName, [FromBody] ApiServiceDto apiServiceDto)
        {
            try
            {
                var fileResult = await _dynamicApiConfigurationService.UpdateAsync(apiServiceName, apiServiceDto);
                if (fileResult == null)
                    return NotFound($"api-сервис: {apiServiceName} не найден");

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

