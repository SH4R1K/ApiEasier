using ApiEasier.Server.Interfaces;
using ApiEasier.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiEasier.Server.Controllers
{
    /// <summary>
    /// Контроллер для обработки действий API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiActionController : ControllerBase
    {
        private readonly IConfigFileApiService _configFileApiService;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ApiActionController"/>.
        /// </summary>
        /// <param name="configFileApiService">Сервис для работы с конфигурационными файлами API.</param>
        public ApiActionController(IConfigFileApiService configFileApiService)
        {
            _configFileApiService = configFileApiService;
        }

        // GET api/ApiAction/{apiServiceName}/{entityName}
        [HttpGet("{apiServiceName}/{entityName}")]
        public async Task<IActionResult> Get(string apiServiceName, string entityName)
        {
            try
            {
                var entity = await _configFileApiService.GetApiEntityAsync(entityName, apiServiceName);
                if (entity == null)
                {
                    return NotFound($"Сущность с именем {entityName} не найдена");
                }

                return Ok(entity.Actions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // GET api/ApiAction/{apiServiceName}/{entityName}/{actionName}
        [HttpGet("{apiServiceName}/{entityName}/{actionName}")]
        public async Task<IActionResult> Get(string apiServiceName, string entityName, string actionName)
        {
            try
            {
                var entity = await _configFileApiService.GetApiEntityAsync(entityName, apiServiceName);
                if (entity == null)
                {
                    return NotFound($"Сущность с именем {entityName} не найдена");
                }

                var action = entity.Actions.FirstOrDefault(a => a.Route == actionName);
                if (action == null)
                {
                    return NotFound($"Действие с именем {actionName} не найдено."); // Возвращаем 404, если действие не найдено
                }
                return Ok(action);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // POST api/ApiAction/{apiServiceName}/{entityName}
        [HttpPost("{apiServiceName}/{entityName}")]
        public async Task<IActionResult> Post(string apiServiceName, string entityName, [FromBody] ApiAction newAction)
        {
            try
            {
                var apiService = await _configFileApiService.DeserializeApiServiceAsync(apiServiceName);

                // Проверка существования файла
                if (apiService == null)
                {
                    return NotFound($"Файл {apiServiceName}.json не существует."); // Возвращаем 404, если файл не найден
                }

                var entity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);

                if (entity == null)
                {
                    return NotFound($"Сущность с именем {entityName} не найдена");
                }

                // Проверка на уникальность имени действия
                if (entity.Actions.Any(a => a.Route == newAction.Route))
                {
                    return Conflict($"Действие с именем {newAction.Route} уже существует."); // Возвращаем 409, если действие уже существует
                }

                // Добавление нового действия
                entity.Actions.Add(newAction);

                if (!_configFileApiService.IsApiServiceExist(apiServiceName))
                    return Conflict($"Файл {apiServiceName}.json не существует.");
                await _configFileApiService.SerializeApiServiceAsync(apiServiceName, apiService);

                return CreatedAtAction(nameof(Get), new { apiServiceName, entityName, actionName = newAction.Route }, newAction);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // PUT api/ApiAction/{apiServiceName}/{entityName}/{actionName}
        [HttpPut("{apiServiceName}/{entityName}/{actionName}")]
        public async Task<IActionResult> Put(string apiServiceName, string entityName, string actionName, [FromBody] ApiAction updatedAction)
        {
            try
            {
                var apiService = await _configFileApiService.DeserializeApiServiceAsync(apiServiceName);

                // Проверка существования файла
                if (apiService == null)
                {
                    return NotFound($"Файл {apiServiceName}.json не существует."); // Возвращаем 404, если файл не найден
                }

                var entity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);

                if (entity == null)
                {
                    return NotFound($"Сущность с именем {entityName} не найдена");
                }

                // Поиск существующего действия
                var existingAction = entity.Actions.FirstOrDefault(a => a.Route == actionName);
                if (existingAction == null)
                {
                    return NotFound($"Действие с именем {actionName} не найдено."); // Возвращаем 404, если действие не найдено
                }

                // Обновление действия
                existingAction.Route = updatedAction.Route; // Обновите свойства по мере необходимости
                existingAction.IsActive = updatedAction.IsActive;
                existingAction.Type = updatedAction.Type;

                if (!_configFileApiService.IsApiServiceExist(apiServiceName))
                    return Conflict($"Файл {apiServiceName}.json не существует.");
                await _configFileApiService.SerializeApiServiceAsync(apiServiceName, apiService);

                return NoContent(); // Возвращаем 204 No Content, так как обновление прошло успешно
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // DELETE api/ApiAction/{apiServiceName}/{entityName}/{actionName}
        [HttpDelete("{apiServiceName}/{entityName}/{actionName}")]
        public async Task<IActionResult> Delete(string apiServiceName, string entityName, string actionName)
        {
            try
            {
                var apiService = await _configFileApiService.DeserializeApiServiceAsync(apiServiceName);

                // Проверка существования файла
                if (apiService == null)
                {
                    return NotFound($"Файл {apiServiceName}.json не существует."); // Возвращаем 404, если файл не найден
                }

                var entity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);

                if (entity == null)
                {
                    return NotFound($"Сущность с именем {entityName} не найдена");
                }

                // Поиск существующего действия
                var actionToRemove = entity.Actions.FirstOrDefault(a => a.Route == actionName);
                if (actionToRemove == null)
                {
                    return NotFound($"Действие с именем {actionName} не найдено."); // Возвращаем 404, если действие не найдено
                }

                // Удаление действия
                entity.Actions.Remove(actionToRemove);

                if (!_configFileApiService.IsApiServiceExist(apiServiceName))
                    return Conflict($"Файл {apiServiceName}.json не существует.");
                await _configFileApiService.SerializeApiServiceAsync(apiServiceName, apiService);

                return NoContent(); // Возвращаем 204 No Content, так как удаление прошло успешно
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        //PATCH api/ApiService/{apiServiceName}/{apiEntityName}/{apiActionName}/{isActive}
        [HttpPatch("{apiServiceName}/{apiEntityName}/{apiActionName}/{isActive}")]
        public async Task<IActionResult> ChangeActiveApiEntity(bool isActive, string apiServiceName, string apiEntityName, string apiActionName)
        {
            try
            {
                var apiService = await _configFileApiService.DeserializeApiServiceAsync(apiServiceName);

                // Проверка существования файла
                if (apiService == null)
                {
                    return NotFound($"Файл {apiServiceName}.json не существует."); // Возвращаем 404, если файл не найден
                }

                var entity = apiService.Entities.FirstOrDefault(e => e.Name == apiEntityName);

                if (entity == null)
                {
                    return NotFound($"Сущность с именем {apiEntityName} не найдена");
                }

                // Поиск существующего действия
                var existingAction = entity.Actions.FirstOrDefault(a => a.Route == apiActionName);
                if (existingAction == null)
                {
                    return NotFound($"Действие с именем {apiActionName} не найдено."); // Возвращаем 404, если действие не найдено
                }

                // Обновление действия
                existingAction.IsActive = isActive;

                if (!_configFileApiService.IsApiServiceExist(apiServiceName))
                    return Conflict($"Файл {apiServiceName}.json не существует.");
                await _configFileApiService.SerializeApiServiceAsync(apiServiceName, apiService);

                return NoContent(); // Возвращаем 204 No Content, так как обновление прошло успешно
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }
    }
}

