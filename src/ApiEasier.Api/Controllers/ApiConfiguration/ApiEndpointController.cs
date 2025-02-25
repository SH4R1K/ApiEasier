using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Logger.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApiEasier.Api.Controllers.ApiConfiguration
{
    /// <summary>
    /// Контроллер для обработки действий API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEndpointController(
        IDynamicEndpointConfigurationService dynamicEndpointConfigurationService,
        ILoggerService logger) : ControllerBase
    {
        private readonly IDynamicEndpointConfigurationService _dynamicEndpointConfigurationService = dynamicEndpointConfigurationService;
        private readonly ILoggerService _logger = logger;

        /// <summary>
        /// Возвращет все эндпоинты указанной сущности
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности с требуемыми эндпоинтами</param>
        [HttpGet("{apiServiceName}/{apiEntityName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<List<ApiEndpointDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllEndpoints(
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiServiceName,
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")]
            string apiEntityName)
        {
            try
            {
                var result = await _dynamicEndpointConfigurationService.GetAllAsync(apiServiceName, apiEntityName);

                return Ok(result);
            }
            catch (NullReferenceException ex)
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
        /// Возвращает эндпоинт указанной сущности по имени
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности с указанным эндпоинтом</param>
        /// <param name="apiEndpointName">Имя требуемого эндпоинта</param>
        [HttpGet("{apiServiceName}/{apiEntityName}/{apiEndpointName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<ApiEndpointDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEndpointByName(
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiServiceName,
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiEntityName,
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiEndpointName)
        {
            try
            {
                var result = await _dynamicEndpointConfigurationService.GetByIdAsync(apiServiceName, apiEntityName, apiEndpointName);
                if (result == null)
                    return NotFound($"Эндпоинт {apiEndpointName} не найден");

                return Ok(result);
            }
            catch (NullReferenceException ex)
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
        /// Создает эндпоинт в указанной сущности
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности с указанным эндпоинтом</param>
        /// <param name="apiEndpoint">Данные нового эндпоинта</param>
        [HttpPost("{apiServiceName}/{apiEntityName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<ApiEndpointDto>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateEndpoint(
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiServiceName,
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiEntityName,
            [FromBody] ApiEndpointDto apiEndpoint)
        {
            try
            {
                var result = await _dynamicEndpointConfigurationService.CreateAsync(apiServiceName, apiEntityName, apiEndpoint);
                if (result == null)
                    return Conflict($"Эндпоинт с именем {apiEndpoint.Route} уже существует");

                return CreatedAtAction(nameof(GetEndpointByName), 
                    new { apiServiceName, apiEntityName, apiEndpointName = apiEndpoint.Route }, result);
            }
            catch (NullReferenceException ex)
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
        /// Изменяет эндпоинт у указанной сущности
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности с указанным эндпоинтом</param>
        /// <param name="apiEndpointName">Имя изменяемого эндпоинта</param>
        /// <param name="apiEndpointDto">Новые данные эндпоинта</param>
        [HttpPut("{apiServiceName}/{apiEntityName}/{apiEndpointName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<ApiEndpointDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateEndpoint(
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiServiceName,
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiEntityName,
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiEndpointName,
            [FromBody] ApiEndpointDto apiEndpointDto)
        {
            try
            {
                var result = await _dynamicEndpointConfigurationService
                    .UpdateAsync(apiServiceName, apiEntityName, apiEndpointName, apiEndpointDto);

                return Ok(result);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Удаляет эндпоинт у указанной сущности по имени
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности с указанным эндпоинтом</param>
        /// <param name="apiEndpointName">Имя изменяемого эндпоинта</param>
        [HttpDelete("{apiServiceName}/{apiEntityName}/{apiEndpointName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEndpoint(
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiServiceName,
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiEntityName,
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiEndpointName)
        {
            try
            {
                await _dynamicEndpointConfigurationService.DeleteAsync(apiServiceName, apiEntityName, apiEndpointName);

                return NoContent();
            }
            catch (NullReferenceException ex)
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
        /// Изменяет активность у эндпоинта у указанной сущности
        /// </summary>
        /// <param name="isActive">True, если надо сделать активным эндпоинт, false - неактивным.</param>
        /// <param name="apiServiceName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности с указанным эндпоинтом</param>
        /// <param name="apiEndpointName">Имя изменяемого эндпоинта</param>
        [HttpPatch("{apiServiceName}/{apiEntityName}/{apiEndpointName}/{isActive}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeActiveApiEndpoint(
            bool isActive,
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiServiceName,
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiEntityName,
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiEndpointName)
        {
            try
            {
                await _dynamicEndpointConfigurationService
                    .ChangeActiveStatusAsync(apiServiceName, apiEntityName, apiEndpointName, isActive);

                return NoContent();
            }
            catch (NullReferenceException ex)
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

