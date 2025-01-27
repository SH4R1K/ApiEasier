using ApiEasier.Server.Dto;
using ApiEasier.Server.Interfaces;
using ApiEasier.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiEasier.Server.Controllers
{
    /// <summary>
    /// Контроллер для управления API сервисами.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiServiceController : ControllerBase
    {
        private readonly IConfigFileApiService _configFileApiService;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ApiServiceController"/>.
        /// </summary>
        /// <param name="configFileApiService">Сервис для работы с конфигурационными файлами API.</param>
        public ApiServiceController(IConfigFileApiService configFileApiService)
        {
            _configFileApiService = configFileApiService;
        }

        // GET api/ApiService
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            try
            {
                var serviceNames = _configFileApiService.GetApiServiceNames();
                return Ok(serviceNames);
            }
            catch (Exception ex)
            {
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
        [HttpGet("api/[controller]/getallwithdata")]
        public async Task<IActionResult> GetAllWithData()
        {
            try
            {
                return Ok(await _configFileApiService.GetAllServicesAsync());
            }
            catch (Exception ex)
            {
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // GET api/ApiService/{name}
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                var apiServiceDto = await _configFileApiService.GetApiServiceByNameAsync(name);

                if (apiServiceDto == null)
                {
                    return NotFound($"Файл {name}.json не существует.");
                }

                return Ok(apiServiceDto);
            }
            catch (Exception ex)
            {
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // POST api/ApiService
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApiServiceDto apiServiceDto)
        {
            if (apiServiceDto == null)
            {
                return BadRequest("Данные API сервиса отсутствуют.");
            }

            try
            {
                string apiServiceName = apiServiceDto.Name;

                if (_configFileApiService.IsApiServiceExist(apiServiceName))
                {
                    return Conflict($"Файл {apiServiceName}.json уже существует.");
                }

                var apiService = new ApiService
                {
                    IsActive = apiServiceDto.IsActive,
                    Entities = apiServiceDto.Entities,
                };

                await _configFileApiService.SerializeApiServiceAsync(apiServiceName, apiService);

                return CreatedAtAction(nameof(Get), new { name = apiServiceName }, apiServiceDto);
            }
            catch (Exception ex)
            {
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // PUT api/ApiService/{oldName}
        [HttpPut("{oldName}")]
        public async Task<IActionResult> Put(string oldName, [FromBody] ApiServiceDto apiServiceDto)
        {
            if (apiServiceDto == null)
            {
                return BadRequest("Данные API сервиса отсутствуют.");
            }

            try
            {
                var apiService = await _configFileApiService.DeserializeApiServiceAsync(oldName);

                if (apiService == null)
                {
                    return NotFound($"Файл {oldName}.json не существует.");
                }

                apiService.IsActive = apiServiceDto.IsActive;
                apiService.Description = apiServiceDto.Description;

                await _configFileApiService.SerializeApiServiceAsync(oldName, apiService);
                _configFileApiService.RenameApiService(oldName, apiServiceDto);

                return NoContent();
            }
            catch (Exception ex)
            {
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // DELETE api/ApiService/{apiServiceName}
        [HttpDelete("{apiServiceName}")]
        public IActionResult Delete(string apiServiceName)
        {
            try
            {
                if (!_configFileApiService.IsApiServiceExist(apiServiceName))
                {
                    return Conflict($"Файл {apiServiceName}.json не существует.");
                }

                _configFileApiService.DeleteApiService(apiServiceName);
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

