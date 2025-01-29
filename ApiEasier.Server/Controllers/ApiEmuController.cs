﻿using ApiEasier.Bll.Interfaces;
using ApiEasier.Dm.Models;
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
        private readonly IEmuApiValidationService _apiServiceValidator;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ApiEmuController"/>.
        /// </summary>
        /// <param name="dynamicCollectionService">Сервис для операций с динамическими коллекциями.</param>
        /// <param name="apiServiceValidator">Сервис для валидации API.</param>
        public ApiEmuController(
            IDynamicApiService dynamicCollectionService,
            IEmuApiValidationService apiServiceValidator)
        {
            _dynamicApiService = dynamicCollectionService;
            _apiServiceValidator = apiServiceValidator;
        }

        // GET api/ApiEmu/{apiName}/{entityName}/{endpoint}
        [HttpGet("{apiName}/{entityName}/{endpoint}")]
        public async Task<IActionResult> Get(string apiName, string entityName, string endpoint, [FromQuery] string? filters)
        {
            try
            {
                // проверка существование в конфиге

                // работа с бд


                //var (isValid, _, _) = await _apiServiceValidator.ValidateApiAsync(apiName, entityName, endpoint, TypeResponse.Get);
                //if (!isValid)
                //    return NotFound($"Не найден путь {apiName}/{entityName}/{endpoint}");

                var data = await _dynamicApiService.GetDataAsync($"{apiName}_{entityName}", filters);
                if (data != null)
                    return Ok(data); // Сериализуем результат из dictionary в json
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
                //var (isValid, _, _) = await _apiServiceValidator.ValidateApiAsync(apiName, entityName, endpoint, TypeResponse.GetByIndex);
                //if (!isValid)
                //    return NotFound($"Не найден путь {apiName}/{entityName}/{endpoint}");

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
                //var (isValid, _, entity) = await _apiServiceValidator.ValidateApiAsync(apiName, entityName, endpoint, TypeResponse.Post);
                //if (!isValid)
                //    return NotFound($"Не найден путь {apiName}/{entityName}/{endpoint}");

                // Валидация структуры для сущности
                //isValid = await _apiServiceValidator.ValidateEntityStructureAsync(entity!, json);
                //if (!isValid)
                //    return BadRequest();

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
                // Валидация API, сущности и пути
                //var (isValid, _, entity) = await _apiServiceValidator.ValidateApiAsync(apiName, entityName, endpoint, TypeResponse.Put);
                //if (!isValid)
                //    return NotFound($"Не найден путь {apiName}/{entityName}/{endpoint}");

                //// Валидация структуры для сущности
                //isValid = await _apiServiceValidator.ValidateEntityStructureAsync(entity!, json);
                //if (!isValid)
                //    return BadRequest();

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
                //var (isValid, _, _) = await _apiServiceValidator.ValidateApiAsync(apiName, entityName, endpoint, TypeResponse.Delete);
                //if (!isValid)
                //    return NotFound($"Не найден путь {apiName}/{entityName}/{endpoint}");

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


