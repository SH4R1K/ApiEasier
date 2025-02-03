using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using Microsoft.AspNetCore.Mvc;

namespace ApiEasier.Api.Controllers.ApiConfiguration
{

    // Возможно приедтся изменить тип у создания и изменения чтобы было без entity (добавить dto и converter)
    [Route("api/[controller]")]
    [ApiController]
    public class ApiServiceController : ControllerBase
    {
        private readonly IDynamicApiConfigurationService _dynamicApiConfigurationService;

        public ApiServiceController(IDynamicApiConfigurationService dynamicApiConfigurationService)
        {
            _dynamicApiConfigurationService = dynamicApiConfigurationService;
        }

        // GET api/ApiService
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromQuery] string? searchTerm, [FromQuery] int? pageSize)
        {
            try
            {
                var result = await _dynamicApiConfigurationService.GetAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // GET api/ApiService/{name}
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var result = await _dynamicApiConfigurationService.GetByIdAsync(name);

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
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        // PUT api/ApiService/{oldName}
        [HttpPut("{oldName}")]
        public async Task<IActionResult> Put(string name, [FromBody] ApiServiceDto apiServiceDto)
        {
            try
            {
                var result = await _dynamicApiConfigurationService.UpdateAsync(name, apiServiceDto);
                return Ok(result);
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
                var result = _dynamicApiConfigurationService.Delete(apiServiceName);
                if (result)
                    return NoContent();

                return NotFound();
            }
            catch (Exception ex)
            {
                // Логирование исключения (не показано здесь)
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        //PATCH api/ApiService/{apiServiceName}/{isActive}
        //[HttpPatch("{apiServiceName}/{isActive}")]
        //public async Task<IActionResult> ChangeActiveApiService(bool isActive, string apiServiceName)
        //{
        //    try
        //    {
        //        var apiService = await _dynamicApiConfigurationService.DeserializeApiServiceAsync(apiServiceName);

        //        if (apiService == null)
        //        {
        //            return NotFound($"Файл {apiServiceName}.json не существует.");
        //        }

        //        apiService.IsActive = isActive;

        //        await _dynamicApiConfigurationService.SerializeApiServiceAsync(apiServiceName, apiService);
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Логирование исключения (не показано здесь)
        //        return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
        //    }
        //}
    }
}

