using ApiEasier.Bll.Interfaces;
using ApiEasier.Dm.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiEasier.Api.Controllers
{
    /// <summary>
    /// Контроллер для управления сущностями API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEntityController : ControllerBase
    {
        private readonly IConfigFileApiService _configFileApiService;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ApiEntityController"/>.
        /// </summary>
        /// <param name="configFileApiService">Сервис для работы с конфигурационными файлами API.</param>
        public ApiEntityController(IConfigFileApiService configFileApiService)
        {
            _configFileApiService = configFileApiService;
        }

        // GET api/ApiEntity/{apiServiceName}
        [HttpGet("{apiServiceName}")]
        public async Task<IActionResult> Get(string apiServiceName)
        {
            try
            {
                var apiService = await _configFileApiService.DeserializeApiServiceAsync(apiServiceName);

                if (apiService == null)
                {
                    return NotFound($"Файл {apiServiceName}.json не существует."); // Возвращаем 404, если файл не найден
                }

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
        public async Task<IActionResult> Get(string apiServiceName, string entityName)
        {
            try
            {
                var apiService = await _configFileApiService.DeserializeApiServiceAsync(apiServiceName);

                if (apiService == null)
                {
                    return NotFound($"Файл {apiServiceName}.json не существует."); // Возвращаем 404, если файл не найден
                }

                var entity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);
                if (entity == null)
                {
                    return NotFound($"Сущность с именем {entityName} не найдена."); // Возвращаем 404, если сущность не найдена
                }
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
        public async Task<IActionResult> Post(string apiServiceName, [FromBody] ApiEntity newEntity)
        {
            if (newEntity == null)
            {
                return BadRequest("Данные сущности отсутствуют."); // Возвращаем 400, если данные отсутствуют
            }

            try
            {
                var apiService = await _configFileApiService.DeserializeApiServiceAsync(apiServiceName);

                if (apiService == null)
                {
                    return NotFound($"Файл {apiServiceName}.json не существует."); // Возвращаем 404, если файл не найден
                }

                if (apiService.Entities.Any(e => e.Name == newEntity.Name))
                {
                    return Conflict($"Сущность с именем {newEntity.Name} уже существует."); // Возвращаем 409, если сущность уже существует
                }

                apiService.Entities.Add(newEntity);
                await _configFileApiService.SerializeApiServiceAsync(apiServiceName, apiService);

                return CreatedAtAction(nameof(Get), new { apiServiceName, entityName = newEntity.Name }, newEntity);
            }
            catch (Exception ex)
            {
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // PUT api/ApiEntity/{apiServiceName}/{entityName}
        [HttpPut("{apiServiceName}/{entityName}")]
        public async Task<IActionResult> Put(string apiServiceName, string entityName, [FromBody] ApiEntity updatedEntity)
        {
            if (updatedEntity == null)
            {
                return BadRequest("Данные сущности отсутствуют."); // Возвращаем 400, если данные отсутствуют
            }

            try
            {
                var apiService = await _configFileApiService.DeserializeApiServiceAsync(apiServiceName);

                if (apiService == null)
                {
                    return NotFound($"Файл {apiServiceName}.json не существует."); // Возвращаем 404, если файл не найден
                }

                var existingEntity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);
                if (existingEntity == null)
                {
                    return NotFound($"Сущность с именем {entityName} не найдена."); // Возвращаем 404, если сущность не найдена
                }

                // Обновление сущности
                existingEntity.Name = updatedEntity.Name; // Обновите свойства по мере необходимости
                existingEntity.IsActive = updatedEntity.IsActive;
                existingEntity.Structure = updatedEntity.Structure;

                await _configFileApiService.SerializeApiServiceAsync(apiServiceName, apiService);

                return NoContent(); // Возвращаем 204 No Content, так как обновление прошло успешно
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
                var apiService = await _configFileApiService.DeserializeApiServiceAsync(apiServiceName);

                if (apiService == null)
                {
                    return NotFound($"Файл {apiServiceName}.json не существует."); // Возвращаем 404, если файл не найден
                }

                var entityToRemove = apiService.Entities.FirstOrDefault(e => e.Name == entityName);
                if (entityToRemove == null)
                {
                    return NotFound($"Сущность с именем {entityName} не найдена."); // Возвращаем 404, если сущность не найдена
                }

                // Удаление сущности
                apiService.Entities.Remove(entityToRemove);

                await _configFileApiService.SerializeApiServiceAsync(apiServiceName, apiService);

                return NoContent(); // Возвращаем 204 No Content, так как удаление прошло успешно
            }
            catch (Exception ex)
            {
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        //PATCH api/ApiService/{apiServiceName}/{apiEntityName}/{isActive}
        [HttpPatch("{apiServiceName}/{apiEntityName}/{isActive}")]
        public async Task<IActionResult> ChangeActiveApiEntity(bool isActive, string apiServiceName, string apiEntityName)
        {
            try
            {
                var apiService = await _configFileApiService.DeserializeApiServiceAsync(apiServiceName);

                if (apiService == null)
                {
                    return NotFound($"Файл {apiServiceName}.json не существует."); // Возвращаем 404, если файл не найден
                }

                var existingEntity = apiService.Entities.FirstOrDefault(e => e.Name == apiEntityName);
                if (existingEntity == null)
                {
                    return NotFound($"Сущность с именем {apiEntityName} не найдена."); // Возвращаем 404, если сущность не найдена
                }

                // Обновление сущности
                existingEntity.IsActive = isActive;

                await _configFileApiService.SerializeApiServiceAsync(apiServiceName, apiService);

                return NoContent(); // Возвращаем 204 No Content, так как обновление прошло успешно
            }
            catch (Exception ex)
            {
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
    }
}

