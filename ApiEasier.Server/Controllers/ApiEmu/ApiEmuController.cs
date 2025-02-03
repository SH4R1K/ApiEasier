using ApiEasier.Bll.Interfaces.ApiEmu;
using Microsoft.AspNetCore.Mvc;

namespace ApiEasier.Api.Controllers.ApiEmu
{
    /// <summary>
    /// Контроллер для обработки запросов эмуляции API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEmuController : ControllerBase
    {
        private readonly IDynamicResource _dynamicResource;
        private readonly IValidatorDynamicApiService _validatorDynamicApiService;

        public ApiEmuController(
            IDynamicResource dynamicResourceService,
            IValidatorDynamicApiService validatorDynamicApiService)
        {
            _dynamicResource = dynamicResourceService;
            _validatorDynamicApiService = validatorDynamicApiService;
        }

        // GET api/ApiEmu/{apiName}/{entityName}/{endpoint}
        [HttpGet("{apiName}/{entityName}/{endpoint}")]
        public async Task<IActionResult> Get(string apiName, string entityName, string endpoint, [FromQuery] string? filters)
        {
            try
            {
                //проверка существование в конфиге
                var (isValid, _, _) = await _validatorDynamicApiService.ValidateApiAsync(apiName, entityName, endpoint, "get");
                if (!isValid)
                    return NotFound($"Не найден путь {apiName}/{entityName}/{endpoint}");

                // работа с бд
                var data = await _dynamicResource.GetDataAsync(apiName, entityName, filters);
                if (data != null)
                    return Ok(data);
                else
                    return NotFound("Не найдены данные");
            }
            catch (Exception ex)
            {
                // Логирование исключения
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // GET api/ApiEmu/{apiName}/{entityName}/{endpoint}/{id}
        [HttpGet("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> GetById(string apiName, string entityName, string endpoint, string id, [FromQuery] string? filters)
        {
            try
            {
                var (isValid, _, _) = await _validatorDynamicApiService.ValidateApiAsync(apiName, entityName, endpoint, "getbyindex");
                if (!isValid)
                    return NotFound($"Не найден путь {apiName}/{entityName}/{endpoint}");

                var result = await _dynamicResource.GetDataByIdAsync(apiName, entityName, id, filters);
                if (result != null)
                    return Ok(result);
                else
                    return NotFound("Не найдены данные");
            }
            catch (Exception ex)
            {
                // Логирование исключения
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // POST api/ApiEmu/{apiName}/{entityName}/{endpoint}
        [HttpPost("{apiName}/{entityName}/{endpoint}")]
        public async Task<IActionResult> Post(string apiName, string entityName, string endpoint, object json)
        {
            try
            {
                var (isValid, _, entity) = await _validatorDynamicApiService.ValidateApiAsync(apiName, entityName, endpoint, "post");
                if (!isValid)
                    return NotFound($"Не найден путь {apiName}/{entityName}/{endpoint}");

                //Валидация структуры для сущности
                isValid = await _validatorDynamicApiService.ValidateEntityStructureAsync(entity!, json);
                if (!isValid)
                    return BadRequest();

                var result = await _dynamicResource.AddDataAsync(apiName, entityName, json);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Логирование исключения
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // PUT api/ApiEmu/{apiName}/{entityName}/{endpoint}/{id}
        [HttpPut("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> Put(string apiName, string entityName, string endpoint, string id, object json)
        {
            try
            {
                //Валидация API, сущности и пути
                var (isValid, _, entity) = await _validatorDynamicApiService.ValidateApiAsync(apiName, entityName, endpoint, "put");
                if (!isValid)
                    return NotFound($"Не найден путь {apiName}/{entityName}/{endpoint}");

                // Валидация структуры для сущности
                isValid = await _validatorDynamicApiService.ValidateEntityStructureAsync(entity!, json);
                if (!isValid)
                    return BadRequest();

                var result = await _dynamicResource.UpdateDataAsync(apiName, entityName, id, json);

                if (result != null)
                    return Ok(result);
                else
                    return NotFound($"Не найдены данные");
            }
            catch (Exception ex)
            {
                // Логирование исключения
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        // DELETE api/ApiEmu/{apiName}/{entityName}/{endpoint}/{id}
        [HttpDelete("{apiName}/{entityName}/{endpoint}/{id}")]
        public async Task<IActionResult> Delete(string apiName, string entityName, string endpoint, string id)
        {
            try
            {
                var (isValid, _, _) = await _validatorDynamicApiService.ValidateApiAsync(apiName, entityName, endpoint, "delete");

                if (!isValid)
                    return NotFound($"Не найден путь {apiName}/{entityName}/{endpoint}");

                var result = await _dynamicResource.DeleteDataAsync(apiName, entityName, id);

                if (result)
                    return NoContent();

                return NotFound();
            }
            catch (Exception ex)
            {
                // Логирование исключения
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }
    }
}


