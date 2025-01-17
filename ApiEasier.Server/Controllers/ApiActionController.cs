using ApiEasier.Server.Dto;
using ApiEasier.Server.Models;
using ApiEasier.Server.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiEasier.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiActionController : ControllerBase
    {
        private JsonService _jsonService;

        public ApiActionController(JsonService jsonService)
        {
            _jsonService = jsonService;
        }

        // GET: api/<ApiEntityController>
        [HttpGet("{apiServiceName}/{entityName}")]
        public async Task<IActionResult> Get(string apiServiceName, string entityName)
        {
            var entity = await _jsonService.GetApiEntity(entityName, apiServiceName);
            if (entity == null) 
            {
                return NotFound($"Cущность с именем {entityName} не найдена");
            }

            return Ok(entity.Actions);
        }

        // GET api/<ApiActionController>/5
        [HttpGet("{apiServiceName}/{entityName}/{actionName}")]
        public async Task<IActionResult> Get(string apiServiceName, string entityName, string actionName)
        {
            var entity = await _jsonService.GetApiEntity(entityName, apiServiceName);
            if (entity == null)
            {
                return NotFound($"Cущность с именем {entityName} не найдена");
            }

            var action = entity.Actions.FirstOrDefault(a => a.Route == actionName);
            if (action == null)
            {
                return NotFound($"Действие с именем {actionName} не найдено."); // Возвращаем 404, если действие не найдено
            }
            return Ok(action);
        }

        // POST api/<ApiActionController>
        [HttpPost("{apiServiceName}/{entityName}")]
        public async Task<IActionResult> Post(string apiServiceName, string entityName, [FromBody] ApiAction newAction)
        {
            var apiService = await _jsonService.DeserializeApiServiceAsync(apiServiceName);

            // Проверка существования файла
            if (apiService == null)
            {
                return NotFound($"Файл {apiServiceName}.json не существует."); // Возвращаем 404, если файл не найден
            }

            var entity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);

            if (entity == null)
            {
                return NotFound($"Cущность с именем {entityName} не найдена");
            }

            // Проверка на уникальность имени действия
            if (entity.Actions.Any(a => a.Route == newAction.Route))
            {
                return Conflict($"Действие с именем {newAction.Route} уже существует."); // Возвращаем 409, если действие уже существует
            }

            // Добавление нового действия
            entity.Actions.Add(newAction);

            // Сериализация обновленного объекта в JSON
            try
            {
                await _jsonService.SerializeApiServiceAsync(apiServiceName, apiService);
            }
            catch (Exception)
            {
                return Conflict($"Файл {apiServiceName}.json уже существует.");
            }

            return CreatedAtAction(nameof(Get), new { apiServiceName, entityName, actionName = newAction.Route }, newAction);
        }

        // PUT api/<ApiActionController>/5
        [HttpPut("{apiServiceName}/{entityName}/{actionName}")]
        public async Task<IActionResult> Put(string apiServiceName, string entityName, string actionName, [FromBody] ApiAction updatedAction)
        {
            var apiService = await _jsonService.DeserializeApiServiceAsync(apiServiceName);

            // Проверка существования файла
            if (apiService == null)
            {
                return NotFound($"Файл {apiServiceName}.json не существует."); // Возвращаем 404, если файл не найден
            }

            var entity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);

            if (entity == null)
            {
                return NotFound($"Cущность с именем {entityName} не найдена");
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

            // Сериализация обновленного объекта в JSON
            await _jsonService.SerializeApiServiceAsync(apiServiceName, apiService);

            return NoContent(); // Возвращаем 204 No Content, так как обновление прошло успешно
        }

        // DELETE api/<ApiActionController>/5
        [HttpDelete("{apiServiceName}/{entityName}/{actionName}")]
        public async Task<IActionResult> Delete(string apiServiceName, string entityName, string actionName)
        {
            var apiService = await _jsonService.DeserializeApiServiceAsync(apiServiceName);

            // Проверка существования файла
            if (apiService == null)
            {
                return NotFound($"Файл {apiServiceName}.json не существует."); // Возвращаем 404, если файл не найден
            }

            var entity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);

            if (entity == null)
            {
                return NotFound($"Cущность с именем {entityName} не найдена");
            }

            // Поиск существующего действия
            var actionToRemove = entity.Actions.FirstOrDefault(a => a.Route == actionName);
            if (actionToRemove == null)
            {
                return NotFound($"Действие с именем {actionName} не найдено."); // Возвращаем 404, если действие не найдено
            }

            // Удаление действия
            entity.Actions.Remove(actionToRemove);

            // Сериализация обновленного объекта в JSON
            await _jsonService.SerializeApiServiceAsync(apiServiceName, apiService);

            return NoContent(); // Возвращаем 204 No Content, так как удаление прошло успешно
        }
    }
}
