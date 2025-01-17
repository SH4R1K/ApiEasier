using ApiEasier.Server.Dto;
using ApiEasier.Server.Models;
using ApiEasier.Server.Services;
using Microsoft.AspNetCore.Mvc;

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
            return _jsonService.GetApiServiceNames();
        }

        // GET api/<ApiServiceController>/5
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var apiServiceDto = await _jsonService.GetApiServiceByName(name);

            if (apiServiceDto == null)
            {
                return NotFound($"Файл {name}.json не существует."); // Используем NotFound вместо Conflict
            }

            // Возврат объекта в формате JSON
            return Ok(apiServiceDto);
        }

        // POST api/<ApiServiceController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApiServiceDto apiServiceDto)
        {
            // Определение имени файла на основе имени сервиса
            string apiServiceName = apiServiceDto.Name; // Имя сервиса передается в теле запроса

            var apiService = new ApiService
            {
                IsActive = apiServiceDto.IsActive,
                Entities = apiServiceDto.Entities,
            };


            if (!await _jsonService.SerializeApiServiceAsync(apiServiceName, apiService, false))
                return Conflict($"Файл {apiServiceName}.json уже существует.");

            return CreatedAtAction(nameof(Post), new
            {
                name = apiServiceName
            }, apiServiceDto);
        }

        // PUT api/<ApiServiceController>/5
        [HttpPut("{oldName}")]
        public async Task<IActionResult> Put(string oldName, [FromBody] ApiServiceDto apiServiceDto)
        {
            var apiService = await _jsonService.DeserializeApiServiceAsync(oldName);

            // Проверка существования файла
            if (apiService == null)
            {
                return NotFound($"Файл {apiServiceDto.Name}.json не существует."); // Возвращаем 404, если файл не найден
            }

            apiService.IsActive = apiServiceDto.IsActive;

            if (!await _jsonService.SerializeApiServiceAsync(oldName, apiService))
                return Conflict($"Файл {oldName}.json не существует.");

            _jsonService.RenameApiService(oldName, apiServiceDto);

            return NoContent(); // Возвращаем 204 No Content, так как обновление прошло успешно
        }



        // DELETE api/<ApiServiceController>/5
        [HttpDelete("{name}")]
        public IActionResult Delete(string apiServiceName)
        {
            try
            {
                _jsonService.DeleteApiService(apiServiceName);
            }
            catch (Exception)
            {
                return Conflict($"Файл {apiServiceName}.json не существует.");
            }
            return NoContent();
        }
    }
}
