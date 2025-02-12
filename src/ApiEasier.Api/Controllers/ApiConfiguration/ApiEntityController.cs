using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using Microsoft.AspNetCore.Mvc;

namespace ApiEasier.Api.Controllers.ApiConfiguration
{
    /// <summary>
    /// Контроллер для управления сущностями api-сервиса.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEntityController : ControllerBase
    {
        private readonly IDynamicEntityConfigurationService _dynamicEntityConfigurationService;

        public ApiEntityController(
            IDynamicEntityConfigurationService dynamicEntityConfigurationService)
        {
            _dynamicEntityConfigurationService = dynamicEntityConfigurationService;
        }

        // GET api/ApiEntity/{apiServiceName}
        [HttpGet("{apiServiceName}")]
        public async Task<IActionResult> GetEntitiesByApiName(string apiServiceName)
        {
            try
            {
                var apiEntities = await _dynamicEntityConfigurationService.GetAsync(apiServiceName);

                if (apiEntities == null)
                    return NotFound();

                return Ok(apiEntities);
            }
            catch (Exception ex)
            {
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // GET api/ApiEntity/{apiServiceName}/{entityName}
        [HttpGet("{apiServiceName}/{entityName}")]
        public async Task<IActionResult> GetEntityByName(string apiServiceName, string entityName)
        {
            try
            {
                var apiEntity = await _dynamicEntityConfigurationService.GetByIdAsync(apiServiceName, entityName);

                if (apiEntity == null)
                    return NotFound($"Сущность {entityName} в api-сервисе {apiServiceName} не найдена.");

                return Ok(apiEntity);
            }
            catch (Exception ex)
            {
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // POST api/ApiEntity/{apiServiceName}
        [HttpPost("{apiServiceName}")]
        public async Task<IActionResult> CreateEntity(string apiServiceName, [FromBody] ApiEntityDto apiEntityDto)
        {
            try
            {
                var result = await _dynamicEntityConfigurationService.CreateAsync(apiServiceName, apiEntityDto);

                if (result == null)
                    return BadRequest($"Не удалось создать сущность у api-сервиса {apiServiceName}.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // PUT api/ApiEntity/{apiServiceName}/{entityName}
        [HttpPut("{apiServiceName}/{entityName}")]
        public async Task<IActionResult> UpdateEntity(string apiServiceName, string entityName, [FromBody] ApiEntityDto apiEntityDto)
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
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // DELETE api/ApiEntity/{apiServiceName}/{entityName}
        [HttpDelete("{apiServiceName}/{entityName}")]
        public async Task<IActionResult> Delete(string apiServiceName, string entityName)
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
        public async Task<IActionResult> ChangeActiveApiEntity(bool isActive, string apiServiceName, string apiEntityName)
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
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
    }
}

