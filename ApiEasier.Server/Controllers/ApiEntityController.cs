﻿using ApiEasier.Server.Models;
using ApiEasier.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiEasier.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEntityController : ControllerBase
    {
        private JsonService _jsonService;

        public ApiEntityController(JsonService jsonService)
        {
            _jsonService = jsonService;
        }

        // GET: api/<ApiEntityController>
        [HttpGet("{apiName}")]
        public async Task<IActionResult> Get(string apiName)
        {
            var filePath = _jsonService.GetFilePath(apiName);

            // Проверка существования файла
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Файл {apiName}.json не существует."); // Используем NotFound вместо Conflict
            }

            var apiService = await _jsonService.DeserializeApiServiceAsync(filePath);

            return Ok(apiService.Entities);
        }

        

        // GET api/<ApiEntityController>/5
        [HttpGet("{apiName}/{entityName}")]
        public async Task<IActionResult> Get(string apiName, string entityName)
        {
            var filePath = _jsonService.GetFilePath(apiName);

            // Проверка существования файла
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Файл {apiName}.json не существует."); // Используем NotFound вместо Conflict
            }

            var apiService = await _jsonService.DeserializeApiServiceAsync(filePath);

            var entity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);

            return Ok(entity);
        }

        // POST api/<ApiEntityController>
        [HttpPost("{apiName}")]
        public async Task<IActionResult> Post(string apiName, [FromBody] ApiEntity newEntity)
        {
            var filePath = _jsonService.GetFilePath(apiName);

            // Проверка существования файла
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Файл {apiName}.json не существует."); // Возвращаем 404, если файл не найден
            }

            var apiService = await _jsonService.DeserializeApiServiceAsync(filePath);

            // Проверка на уникальность имени сущности
            if (apiService.Entities != null && apiService.Entities.Any(e => e.Name == newEntity.Name))
            {
                return Conflict($"Сущность с именем {newEntity.Name} уже существует."); // Возвращаем 409, если сущность уже существует
            }   

            // Добавление новой сущности
            apiService.Entities.Add(newEntity);

            // Сериализация обновленного объекта в JSON
            await _jsonService.SerializeApiServiceAsync(filePath, apiService);

            return CreatedAtAction("Get", new { apiName, entityName = newEntity.Name }, newEntity);

        }

        // PUT api/<ApiEntityController>/5
        [HttpPut("{apiName}/{entityName}")]
        public async Task<IActionResult> Put(string apiName, string entityName, [FromBody] ApiEntity updatedEntity)
        {
            var filePath = _jsonService.GetFilePath(apiName);

            // Проверка существования файла
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Файл {apiName}.json не существует."); // Возвращаем 404, если файл не найден
            }

            var apiService = await _jsonService.DeserializeApiServiceAsync(filePath);

            // Поиск существующей сущности
            var existingEntity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);
            if (existingEntity == null)
            {
                return NotFound($"Сущность с именем {entityName} не найдена."); // Возвращаем 404, если сущность не найдена
            }

            // Обновление сущности
            existingEntity.Name = updatedEntity.Name; // Обновите свойства по мере необходимости
            existingEntity.IsActive = updatedEntity.IsActive;
            existingEntity.Structure = updatedEntity.Structure;

            // Сериализация обновленного объекта в JSON
            await _jsonService.SerializeApiServiceAsync(filePath, apiService);

            return NoContent(); // Возвращаем 204 No Content, так как обновление прошло успешно
        }

        // DELETE api/<ApiEntityController>/5
        [HttpDelete("{apiName}/{entityName}")]
        public async Task<IActionResult> Delete(string apiName, string entityName)
        {
            var filePath = _jsonService.GetFilePath(apiName);

            // Проверка существования файла
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Файл {apiName}.json не существует."); // Возвращаем 404, если файл не найден
            }

            var apiService = await _jsonService.DeserializeApiServiceAsync(filePath);

            // Поиск существующей сущности
            var entityToRemove = apiService.Entities.FirstOrDefault(e => e.Name == entityName);
            if (entityToRemove == null)
            {
                return NotFound($"Сущность с именем {entityName} не найдена."); // Возвращаем 404, если сущность не найдена
            }

            // Удаление сущности
            apiService.Entities.Remove(entityToRemove);

            // Сериализация обновленного объекта в JSON
            await _jsonService.SerializeApiServiceAsync(filePath, apiService);

            return NoContent(); // Возвращаем 204 No Content, так как удаление прошло успешно
        }
    }
}
