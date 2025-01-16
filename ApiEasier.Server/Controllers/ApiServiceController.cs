using ApiEasier.Server.Dto;
using ApiEasier.Server.Models;
using ApiEasier.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiEasier.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiServiceController : ControllerBase
    {
        private JsonService _jsonService;

        public ApiServiceController(JsonService jsonService)
        {
            _jsonService = jsonService;
        }

        // GET: api/<ApiServiceController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            DirectoryInfo directory = new DirectoryInfo("configuration");
            var files = directory.GetFiles();
            return files.Select(f => Path.GetFileNameWithoutExtension(f.Name));
        }

        // GET api/<ApiServiceController>/5
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            // Определение пути к файлу
            string directoryPath = "configuration";
            string filePath = Path.Combine(directoryPath, name + ".json");

            // Проверка существования файла
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Файл {name}.json не существует."); // Используем NotFound вместо Conflict
            }

            // Чтение содержимого файла
            var json = await System.IO.File.ReadAllTextAsync(filePath);

            // Десериализация JSON в объект
            var apiService = JsonSerializer.Deserialize<ApiService>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Уменьшение регистра полей
                WriteIndented = true // Запись в читаемом формате
            });

            var apiServiceDto = new ApiServiceDto
            {
                Name = name,
                IsActive = apiService.IsActive,
                Entities = apiService.Entities
            };

            // Возврат объекта в формате JSON
            return Ok(apiServiceDto);
        }


        // POST api/<ApiServiceController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApiServiceDto apiServiceDto)
        {
            // Определение имени файла на основе имени сервиса
            string fileName = apiServiceDto.Name; // Имя сервиса передается в теле запроса

            var apiService = new ApiService 
            {
                IsActive = apiServiceDto.IsActive,
                Entities = apiServiceDto.Entities,
            };

            // Сериализация нового объекта в JSON с отступами
            string json = JsonSerializer.Serialize(apiService, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Уменьшение регистра полей
                WriteIndented = true // Запись в читаемом формате
            });

            string filePath = _jsonService.GetFilePath(fileName);

            // Проверка существования файла
            if (System.IO.File.Exists(filePath))
            {
                return Conflict($"Файл {fileName}.json уже существует.");
            }

            // Запись JSON в файл
            await System.IO.File.WriteAllTextAsync(filePath, json);

            return CreatedAtAction(nameof(Post), new { name = fileName }, apiServiceDto);
        }

        // PUT api/<ApiServiceController>/5
        [HttpPut("{oldName}")]
        public async Task<IActionResult> Put(string oldName, [FromBody] ApiServiceDto apiServiceDto)
        {
            string filePath = _jsonService.GetFilePath(oldName);

            // Проверка существования файла
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Файл {apiServiceDto.Name}.json не существует."); // Возвращаем 404, если файл не найден
            }

            var apiService = await _jsonService.DeserializeApiServiceAsync(filePath);

            apiService.IsActive = apiServiceDto.IsActive;

            // Сериализация обновленного объекта в JSON с отступами
            string json = JsonSerializer.Serialize(apiService, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Уменьшение регистра полей
                WriteIndented = true // Запись в читаемом формате
            });

            // Запись JSON в файл
            await System.IO.File.WriteAllTextAsync(filePath, json);

            if (oldName != apiServiceDto.Name)
            {
                // Определение старого и нового путей к файлу
                string oldFilePath = _jsonService.GetFilePath(oldName);
                string newFilePath = _jsonService.GetFilePath(apiServiceDto.Name);

                // Переименование файла
                System.IO.File.Move(oldFilePath, newFilePath);
            }

            return NoContent(); // Возвращаем 204 No Content, так как обновление прошло успешно
        }

        // DELETE api/<ApiServiceController>/5
        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            string filePath = _jsonService.GetFilePath(name);

            // Проверка существования файла
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Файл {name}.json не существует."); // Возвращаем 404, если файл не найден
            }

            System.IO.File.Delete(filePath);

            return NoContent();
        }
    }
}
