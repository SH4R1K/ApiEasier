using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Bll.Interfaces.ApiEmu;
using Microsoft.AspNetCore.Mvc;

namespace ApiEasier.Api.Controllers.ApiConfiguration
{
    /// <summary>
    /// Контроллер для управления сущностями API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEntityController : ControllerBase
    {
        private readonly IDynamicResourceDataService _dynamicApiService;
        private readonly IDynamicApiConfigurationService _dynamicApiConfigurationService;
        private readonly IDynamicEntityConfigurationService _dynamicEntityConfigurationService;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ApiEntityController"/>.
        /// </summary>
        /// <param name="configFileApiService">Сервис для работы с конфигурационными файлами API.</param>
        public ApiEntityController(
            IDynamicResourceDataService dynamicApiService,
            IDynamicApiConfigurationService dynamicApiConfigurationService,
            IDynamicEntityConfigurationService dynamicEntityConfigurationService)
        {
            _dynamicApiService = dynamicApiService;
            _dynamicApiConfigurationService = dynamicApiConfigurationService;
            _dynamicEntityConfigurationService = dynamicEntityConfigurationService;
        }

        // GET api/ApiEntity/{apiServiceName}
        [HttpGet("{apiServiceName}")]
        public async Task<IActionResult> GetEntitiesByApiName(string apiServiceName)
        {
            try
            {
                var apiService = await _dynamicApiConfigurationService.GetByIdAsync(apiServiceName);
               
                if (apiService == null)
                    return NotFound();

                return Ok(apiService.Entities);
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
                var apiService = await _dynamicApiConfigurationService.GetByIdAsync(apiServiceName);
                
                if (apiService == null)
                    return NotFound($"api-сервис: {apiServiceName} не найден");

                var entity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);

                if (entity == null)
                    return NotFound($"Сущность {entityName} в api-сервисе {apiServiceName} не найдена.");

                return Ok(entity);
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
                var apiService = await _dynamicApiConfigurationService.GetByIdAsync(apiServiceName);

                if (apiService == null)
                    return NotFound($"api-сервис: {apiServiceName} не найден");

                if (apiService.Entities.Any(e => e.Name == apiEntityDto.Name))
                    return Conflict($"Сущность с именем {apiEntityDto.Name} уже существует.");

                var result = await _dynamicEntityConfigurationService.CreateAsync(apiServiceName, apiEntityDto); 

                if (!result)
                    return BadRequest($"Не удалось создать сущность у api-сервиса {apiServiceName}.");

                return Ok();
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
                var apiService = await _dynamicApiConfigurationService.GetByIdAsync(apiServiceName);

                if (apiService == null)
                    return NotFound($"api-сервис: {apiServiceName} не найден");

                if (!apiService.Entities.Any(e => e.Name == entityName))
                    return NotFound($"сущность {entityName} у api-сервиса {apiServiceName} не найдена");

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
                var apiService = await _dynamicApiConfigurationService.GetByIdAsync(apiServiceName);

                if (apiService == null)
                    return NotFound($"api-сервис: {apiServiceName} не найден");

                if (!apiService.Entities.Any(e => e.Name == entityName))
                    return NotFound($"сущность {entityName} у api-сервиса {apiServiceName} не найдена");

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
        public async Task<IActionResult> ChangeActiveApiEntity(bool isActive, string apiServiceName, string entityName)
        {
            try
            {
                var apiService = await _dynamicApiConfigurationService.GetByIdAsync(apiServiceName);

                if (apiService == null)
                    return NotFound($"api-сервис: {apiServiceName} не найден");

                if (!apiService.Entities.Any(e => e.Name == entityName))
                    return NotFound($"сущность {entityName} у api-сервиса {apiServiceName} не найдена");

                var result = await _dynamicEntityConfigurationService.ChangeActiveStatusAsync(apiServiceName, entityName, isActive);

                if (!result)
                    return NotFound($"статус у сущности {entityName} у api-сервиса {apiServiceName} не был изменен");

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

