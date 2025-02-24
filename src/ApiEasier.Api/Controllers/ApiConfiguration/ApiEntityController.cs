using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Logger.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApiEasier.Api.Controllers.ApiConfiguration
{
    /// <summary>
    /// Позволяет управлять сущностями API-сервиса.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEntityController(
        IDynamicEntityConfigurationService dynamicEntityConfigurationService,
        ILoggerService logger) : ControllerBase
    {
        private readonly IDynamicEntityConfigurationService _dynamicEntityConfigurationService = dynamicEntityConfigurationService;
        private readonly ILoggerService _logger = logger;

        /// <summary>
        /// Получить все сущности API-сервиса
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса</param>
        [HttpGet("{apiServiceName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<List<ApiEntitySummaryDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEntitiesByApiName(
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")]
            string apiServiceName)
        {
            try
            {
                var apiEntities = await _dynamicEntityConfigurationService.GetAllAsync(apiServiceName);

                if (apiEntities == null)
                    return NotFound($"API-сервис {apiServiceName} не найден");

                return Ok(apiEntities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Возвращает сущность по имени
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса, которому пренадлежит сущность</param>
        /// <param name="entityName">Имя искаемой сущности</param>
        [HttpGet("{apiServiceName}/{entityName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<List<ApiEntitySummaryDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEntityByName(
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")]
            string apiServiceName,
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")]
            string entityName)
        {
            try
            {
                var apiEntity = await _dynamicEntityConfigurationService.GetByIdAsync(apiServiceName, entityName);

                if (apiEntity == null)
                    return NotFound($"Сущность {entityName} в api-сервисе {apiServiceName} не найдена.");

                return Ok(apiEntity);
            }
            catch (NullReferenceException)
            {
                return NotFound($"API-сервис {apiServiceName} не найден");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Добавляет новую сущность API-сервису
        /// </summary>
        /// <param name="apiServiceName">API-сервис, которому надо добавить сущность</param>
        /// <param name="apiEntityDto">Добавляемая сущность</param>
        [HttpPost("{apiServiceName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<List<ApiEntitySummaryDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddEntity(
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")]
            string apiServiceName, [FromBody] ApiEntityDto apiEntityDto)
        {
            try
            {
                var result = await _dynamicEntityConfigurationService.CreateAsync(apiServiceName, apiEntityDto);

                if (result == null)
                    return Conflict($"Сущность с именем {apiEntityDto.Name} уже существует.");

                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return NotFound($"API-сервис {apiServiceName} не найден");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // PUT api/ApiEntity/{apiServiceName}/{entityName}
        [HttpPut("{apiServiceName}/{entityName}")]
        public async Task<IActionResult> UpdateEntity(
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")]
            string apiServiceName,
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string entityName, [FromBody] ApiEntityDto apiEntityDto)
        {
            try
            {
                var result = await _dynamicEntityConfigurationService.UpdateAsync(apiServiceName, entityName, apiEntityDto);

                if (!result)
                    return NotFound($"Сущность {entityName} у api-сервиса {apiServiceName} не удалось обновить");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // DELETE api/ApiEntity/{apiServiceName}/{entityName}
        [HttpDelete("{apiServiceName}/{entityName}")]
        public async Task<IActionResult> Delete(
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiServiceName,
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string entityName)
        {
            try
            {
                var result = await _dynamicEntityConfigurationService.DeleteAsync(apiServiceName, entityName);

                if (!result)
                    return NotFound($"Сущность {entityName} у api-сервиса {apiServiceName} не была удалена");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        //PATCH api/ApiService/{apiServiceName}/{apiEntityName}/{isActive}
        [HttpPatch("{apiServiceName}/{apiEntityName}/{isActive}")]
        public async Task<IActionResult> ChangeActiveApiEntity(
            bool isActive,
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiServiceName,
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")] 
            string apiEntityName)
        {
            try
            {
                var result = await _dynamicEntityConfigurationService.ChangeActiveStatusAsync(apiServiceName, apiEntityName, isActive);

                if (!result)
                    return NotFound($"статус у сущности {apiEntityName} у api-сервиса {apiServiceName} не был изменен");

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

