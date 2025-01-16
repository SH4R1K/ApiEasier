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
        [HttpGet("{apiName}/{entityName}")]
        public async Task<IActionResult> Get(string apiName, string entityName)
        {
            var filePath = _jsonService.GetFilePath(apiName);

            // Проверка существования файла
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Файл {apiName}.json не существует."); // Используем NotFound вместо Conflict
            }

            var entity = await _jsonService.GetApiEntity(entityName, filePath);

            return Ok(entity.Actions);
        }

        // GET api/<ApiActionController>/5
        [HttpGet("{apiName}/{entityName}/{actionName}")]
        public async Task<IActionResult> Get(string apiName, string entityName, string actionName)
        {
            var filePath = _jsonService.GetFilePath(apiName);

            // Проверка существования файла
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Файл {apiName}.json не существует."); // Используем NotFound вместо Conflict
            }

            var entity = await _jsonService.GetApiEntity(entityName, filePath);

            var action = entity.Actions.FirstOrDefault(a => a.Route == actionName);

            return Ok(entity.Actions);
        }

        // POST api/<ApiActionController>
        [HttpPost("{apiName}/{entityName}")]
        public async Task<IActionResult> Post(string apiName, string entityName, [FromBody] ApiAction newAction)
        {
            var filePath = _jsonService.GetFilePath(apiName);

            // Проверка существования файла
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Файл {apiName}.json не существует."); // Возвращаем 404, если файл не найден
            }

            var apiService = await _jsonService.DeserializeApiServiceAsync(filePath);

            var entity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);

            // Проверка на уникальность имени действия
            if (entity.Actions.Any(a => a.Route == newAction.Route))
            {
                return Conflict($"Действие с именем {newAction.Route} уже существует."); // Возвращаем 409, если действие уже существует
            }

            // Добавление нового действия
            entity.Actions.Add(newAction);

            // Сериализация обновленного объекта в JSON
            await _jsonService.SerializeApiServiceAsync(filePath, apiService);

            return CreatedAtAction(nameof(Get), new { apiName, entityName, actionName = newAction.Route }, newAction);
        }

        // PUT api/<ApiActionController>/5
        [HttpPut("{apiName}/{entityName}/{actionName}")]
        public async Task<IActionResult> Put(string apiName, string entityName, string actionName, [FromBody] ApiAction updatedAction)
        {
            var filePath = _jsonService.GetFilePath(apiName);

            // Проверка существования файла
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Файл {apiName}.json не существует."); // Возвращаем 404, если файл не найден
            }

            var apiService = await _jsonService.DeserializeApiServiceAsync(filePath);

            var entity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);

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
            await _jsonService.SerializeApiServiceAsync(filePath, apiService);

            return NoContent(); // Возвращаем 204 No Content, так как обновление прошло успешно
        }

        // DELETE api/<ApiActionController>/5
        [HttpDelete("{apiName}/{entityName}/{actionName}")]
        public async Task<IActionResult> Delete(string apiName, string entityName, string actionName)
        {
            var filePath = _jsonService.GetFilePath(apiName);

            // Проверка существования файла
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Файл {apiName}.json не существует."); // Возвращаем 404, если файл не найден
            }

            var apiService = await _jsonService.DeserializeApiServiceAsync(filePath);

            var entity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);

            // Поиск существующего действия
            var actionToRemove = entity.Actions.FirstOrDefault(a => a.Route == actionName);
            if (actionToRemove == null)
            {
                return NotFound($"Действие с именем {actionName} не найдено."); // Возвращаем 404, если действие не найдено
            }

            // Удаление действия
            entity.Actions.Remove(actionToRemove);

            // Сериализация обновленного объекта в JSON
            await _jsonService.SerializeApiServiceAsync(filePath, apiService);

            return NoContent(); // Возвращаем 204 No Content, так как удаление прошло успешно
        }
    }
}
