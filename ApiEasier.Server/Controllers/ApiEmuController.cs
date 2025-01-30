using ApiEasier.Bll.Interfaces.ApiEmu;
using Microsoft.AspNetCore.Mvc;

namespace ApiEasier.Api.Controllers
{
    /// <summary>
    /// Контроллер для обработки запросов эмуляции API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEmuController : ControllerBase
    {
        private readonly IDynamicApiService _dynamicApiService;
        private readonly IValidatorDynamicApiService _validatorDynamicApiService;

        public ApiEmuController(
            IDynamicApiService dynamicCollectionService,
            IValidatorDynamicApiService validatorDynamicApiService)
        {
            _dynamicApiService = dynamicCollectionService;
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
                var data = await _dynamicApiService.GetDataAsync($"{apiName}_{entityName}", filters);
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

                var result = await _dynamicApiService.GetDocByIdFromCollectionAsync($"{apiName}_{entityName}", id, filters);
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

                var result = await _dynamicApiService.AddDataAsync($"{apiName}_{entityName}", json);
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

                var result = await _dynamicApiService.UpdateDocFromCollectionAsync($"{apiName}_{entityName}", id, json);

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

                var result = await _dynamicApiService.DeleteDocFromCollectionAsync($"{apiName}_{entityName}", id);
                if (result > 0)
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


