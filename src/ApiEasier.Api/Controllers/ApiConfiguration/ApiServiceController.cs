using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Bll.Interfaces.ApiEmu;
using Microsoft.AspNetCore.Mvc;

namespace ApiEasier.Api.Controllers.ApiConfiguration
{

    // Возможно приедтся изменить тип у создания и изменения чтобы было без entity (добавить dto и converter)
    [Route("api/[controller]")]
    [ApiController]
    public class ApiServiceController : ControllerBase
    {
        private readonly IDynamicApiConfigurationService _dynamicApiConfigurationService;

        public ApiServiceController(
            IDynamicApiConfigurationService dynamicApiConfigurationService)
        {
            _dynamicApiConfigurationService = dynamicApiConfigurationService;
        }

        // GET api/ApiService
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _dynamicApiConfigurationService.GetAsync();

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // GET api/ApiService/{name}
        [HttpGet("{apiServiceName}")]
        public async Task<IActionResult> GetByName(string apiServiceName)
        {
            try
            {
                var result = await _dynamicApiConfigurationService.GetByIdAsync(apiServiceName);

                if (result == null)
                    return NotFound($"api-сервис: {apiServiceName} не найден");

                return Ok(result);
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
            try
            {
                var result = await _dynamicApiConfigurationService.CreateAsync(apiServiceDto);

                if (result == null)
                    return NotFound("Не удалось создать api-сервис");
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // PUT api/ApiService/{oldName}
        [HttpPut("{apiServiceName}")]
        public async Task<IActionResult> Put(string apiServiceName, [FromBody] ApiServiceDto apiServiceDto)
        {
            try
            {
                var fileResult = await _dynamicApiConfigurationService.UpdateAsync(apiServiceName, apiServiceDto);
                if (fileResult == null)
                    return NotFound($"api-сервис: {apiServiceName} не найден");

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
        public async Task<IActionResult> Delete(string apiServiceName)
        {
            try
            {
                var result = await _dynamicApiConfigurationService.DeleteAsync(apiServiceName);
                if (!result)
                    return NotFound($"Не удалось найти api-сервис {apiServiceName}");

                return NoContent();
            }
            catch (Exception ex)
            {
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        //PATCH api/ApiService/{apiServiceName}/{isActive}
        [HttpPatch("{apiServiceName}/{isActive}")]
        public async Task<IActionResult> ChangeActiveApiService(bool status, string apiServiceName)
        {
            try
            {
                var result = await _dynamicApiConfigurationService.ChangeActiveStatusAsync(apiServiceName, status);

                if (!result)
                    return NotFound($"Не удалось сменить статус api-сервиса {apiServiceName}");

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

