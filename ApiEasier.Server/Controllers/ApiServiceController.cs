﻿using ApiEasier.Server.Dto;
using ApiEasier.Server.Hubs;
using ApiEasier.Server.Interfaces;
using ApiEasier.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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
        private readonly IHubContext<ApiListHub, IApiListHub> _hubContext;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ApiServiceController"/>.
        /// </summary>
        /// <param name="configFileApiService">Сервис для работы с конфигурационными файлами API.</param>
        public ApiServiceController(IConfigFileApiService configFileApiService, IHubContext<ApiListHub, IApiListHub> hubContext)
        {
            _configFileApiService = configFileApiService;
            _hubContext = hubContext;
        }

        // GET api/ApiService
        [HttpGet]
        public async Task<IActionResult> GetAllWithData([FromQuery] int? page, [FromQuery] string? searchTerm, [FromQuery] int? pageSize)
        {
            try
            {
                var shortApiServices = new List<ShortApiServiceDto>();
                foreach (var apiService in await _configFileApiService.GetApiServicesAsync(page, searchTerm, pageSize))
                {
                    shortApiServices.Add(
                        new ShortApiServiceDto
                        {
                            Name = apiService.Name,
                            IsActive = apiService.IsActive,
                            Description = apiService.Description
                        });
                }
                return Ok(shortApiServices);
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
                    Description = apiServiceDto.Description,
                    Entities = apiServiceDto.Entities,
                };

                await _configFileApiService.SerializeApiServiceAsync(apiServiceName, apiService);
                await _hubContext.Clients.All.AddService(
                            new ShortApiServiceDto
                            {
                                Name = apiServiceDto.Name,
                                IsActive = apiServiceDto.IsActive,
                                Description = apiServiceDto.Description
                            });
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
                await _hubContext.Clients.All.UpdateService(oldName,
                            new ShortApiServiceDto
                            {
                                Name = apiServiceDto.Name,
                                IsActive = apiServiceDto.IsActive,
                                Description = apiServiceDto.Description
                            });
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
                if (!_configFileApiService.IsApiServiceExist(apiServiceName))
                {
                    return Conflict($"Файл {apiServiceName}.json не существует.");
                }

                _configFileApiService.DeleteApiService(apiServiceName);
                await _hubContext.Clients.All.RemoveService(apiServiceName);
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
        public async Task<IActionResult> ChangeActiveApiService(bool isActive, string apiServiceName)
        {
            try
            {
                var apiService = await _configFileApiService.DeserializeApiServiceAsync(apiServiceName);

                if (apiService == null)
                {
                    return NotFound($"Файл {apiServiceName}.json не существует.");
                }

                apiService.IsActive = isActive;

                await _configFileApiService.SerializeApiServiceAsync(apiServiceName, apiService);
                await _hubContext.Clients.All.UpdateStatusService(apiServiceName, isActive);
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

