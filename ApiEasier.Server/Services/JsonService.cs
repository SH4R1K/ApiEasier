using ApiEasier.Server.Dto;
using ApiEasier.Server.Interfaces;
using ApiEasier.Server.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Text.Json;

namespace ApiEasier.Server.Services
{
    /// <summary>
    /// Сервис для получения и редактирования информации про API-сервисы.
    /// </summary>
    public class JsonService : IConfigFileApiService
    {
        private readonly string _path;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _fileSemaphores = new();
        private readonly object _lock = new();
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _config;


        /// <summary>
        /// Базовый конструктор JsonService.
        /// </summary>
        /// <exception cref="InvalidOperationException">Выбрасывается, если не удается создать директорию.</exception>
        public JsonService( IMemoryCache cache, IConfiguration config)
        {
            try
            {
                _config = config;
                _path = _config["JsonDirectoryPath"];
                Directory.CreateDirectory(_path);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new InvalidOperationException("Нет доступа к указанной директории.", ex);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException("Ошибка при создании директории.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Произошла непредвиденная ошибка.", ex);
            }
            _cache = cache;
            _config = config;
        }

        private string GetFilePath(string fileName) => Path.Combine(_path, fileName + ".json");

        private SemaphoreSlim GetSemaphore(string fileName)
        {
            return _fileSemaphores.GetOrAdd(fileName, _ => new SemaphoreSlim(1, 1));
        }

        /// <summary>
        /// Десериализует API-сервис из файла.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса.</param>
        /// <returns>Объект <see cref="ApiService"/> или null, если файл не найден.</returns>
        /// <exception cref="FileNotFoundException">Выбрасывается, если файл не найден.</exception>
        /// <exception cref="DirectoryNotFoundException">Выбрасывается, если директория не найдена.</exception>
        /// <exception cref="PathTooLongException">Выбрасывается, если путь к файлу слишком длинный.</exception>
        /// <exception cref="IOException">Выбрасывается, если произошла ошибка при чтении файла.</exception>
        public async Task<ApiService?> DeserializeApiServiceAsync(string apiServiceName)
        {
            var filePath = GetFilePath(apiServiceName);

            if (!File.Exists(filePath))
                return null;

            var semaphore = GetSemaphore(filePath);

            await semaphore.WaitAsync();

            try
            {
                _cache.TryGetValue(apiServiceName, out ApiService? apiService);
                if (apiService != null)
                    return apiService;

                var json = await File.ReadAllTextAsync(filePath);
                apiService = JsonSerializer.Deserialize<ApiService>(json, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });
                _cache.Set(apiServiceName, apiService, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromHours(1)});
                return apiService;
            }
            catch (FileNotFoundException ex)
            {
                throw new FileNotFoundException("Файл API-сервиса не найден.", ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new DirectoryNotFoundException("Директория для API-сервиса не найдена.", ex);
            }
            catch (PathTooLongException ex)
            {
                throw new PathTooLongException("Путь к файлу слишком длинный.", ex);
            }
            catch (IOException ex)
            {
                throw new IOException("Ошибка при чтении файла API-сервиса.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Произошла непредвиденная ошибка.", ex);
            }
            finally
            {
                semaphore.Release();
            }
        }

        /// <summary>
        /// Сериализует API-сервис в файл.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса.</param>
        /// <param name="apiService">Объект <see cref="ApiService"/> для сериализации.</param>
        /// <exception cref="UnauthorizedAccessException">Выбрасывается, если нет доступа для записи в файл.</exception>
        /// <exception cref="PathTooLongException">Выбрасывается, если путь к файлу слишком длинный.</exception>
        /// <exception cref="IOException">Выбрасывается, если произошла ошибка при записи файла.</exception>
        public async Task SerializeApiServiceAsync(string apiServiceName, ApiService apiService)
        {
            var filePath = GetFilePath(apiServiceName);

            Directory.CreateDirectory(_path);

            var semapthore = GetSemaphore(filePath);

            await semapthore.WaitAsync();
            try
            {
                var json = JsonSerializer.Serialize(apiService, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                await File.WriteAllTextAsync(filePath, json);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("Нет доступа для записи в файл API-сервиса.", ex);
            }
            catch (PathTooLongException ex)
            {
                throw new PathTooLongException("Путь к файлу слишком длинный.", ex);
            }
            catch (IOException ex)
            {
                throw new IOException("Ошибка при записи файла API-сервиса.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Произошла непредвиденная ошибка.", ex);
            }
            finally
            {
                semapthore.Release();
            }
        }

        /// <summary>
        /// Получает сущность API по имени.
        /// </summary>
        /// <param name="entityName">Имя сущности.</param>
        /// <param name="apiServiceName">Имя API-сервиса.</param>
        /// <returns>Объект <see cref="ApiEntity"/> или null, если сущность не найдена.</returns>
        public async Task<ApiEntity?> GetApiEntityAsync(string entityName, string apiServiceName)
        {
            var apiService = await DeserializeApiServiceAsync(apiServiceName);
            return apiService?.Entities.FirstOrDefault(e => e.Name == entityName);
        }

        /// <summary>
        /// Получает имена всех API-сервисов в директории.
        /// </summary>
        /// <returns>Список имен API-сервисов.</returns>
        public IEnumerable<string> GetApiServiceNames()
        {
            lock (_lock)
            {
                DirectoryInfo directory = new DirectoryInfo(_path);
                var files = directory.GetFiles("*.json");
                return files.Select(f => Path.GetFileNameWithoutExtension(f.Name)).ToList();
            }
        }

        /// <summary>
        /// Получает API-сервис по имени.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса.</param>
        /// <returns>Объект <see cref="ApiServiceDto"/> или null, если сервис не найден.</returns>
        public async Task<ApiServiceDto?> GetApiServiceByNameAsync(string apiServiceName)
        {
            var apiService = await DeserializeApiServiceAsync(apiServiceName);
            if (apiService == null)
                return null;

            return new ApiServiceDto
            {
                Name = apiServiceName,
                IsActive = apiService.IsActive,
                Description = apiService.Description,
                Entities = apiService.Entities
            };
        }

        /// <summary>
        /// Переименовывает API-сервис.
        /// </summary>
        /// <param name="oldName">Старое имя API-сервиса.</param>
        /// <param name="apiServiceDto">Объект <see cref="ApiServiceDto"/> с новым именем.</param>
        /// <exception cref="FileNotFoundException">Выбрасывается, если старый файл API-сервиса не найден.</exception>
        /// <exception cref="IOException">Выбрасывается, если произошла ошибка при переименовании файла.</exception>
        /// <exception cref="UnauthorizedAccessException">Выбрасывается, если нет доступа для переименования файла.</exception>
        /// <exception cref="PathTooLongException">Выбрасывается, если путь к файлу слишком длинный при переименовании.</exception>
        public void RenameApiService(string oldName, ApiServiceDto apiServiceDto)
        {
            if (oldName != apiServiceDto.Name)
            {
                var semaphores = new[]
                {
                    (Name: oldName, Semaphore: GetSemaphore(oldName)),
                    (Name: apiServiceDto.Name, Semaphore: GetSemaphore(apiServiceDto.Name))
                }.OrderBy(s => s.Name).ToList();

                foreach (var (_, semaphore) in semaphores)
                    semaphore.Wait();
                try
                {
                    string oldFilePath = GetFilePath(oldName);
                    string newFilePath = GetFilePath(apiServiceDto.Name);

                    File.Move(oldFilePath, newFilePath);

                    // Удаляем старый семафор
                    lock (_lock)
                        _fileSemaphores.TryRemove(oldName, out _);
                }
                catch (FileNotFoundException ex)
                {
                    throw new FileNotFoundException("Старый файл API-сервиса не найден.", ex);
                }
                catch (IOException ex)
                {
                    throw new IOException("Ошибка при переименовании API-сервиса.", ex);
                }
                catch (UnauthorizedAccessException ex)
                {
                    throw new UnauthorizedAccessException("Нет доступа для переименования файла API-сервиса.", ex);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Произошла непредвиденная ошибка.", ex);
                }
                finally
                {
                    foreach (var (_, semaphore) in semaphores)
                        semaphore.Release();
                }
            }
        }

        /// <summary>
        /// Удаляет API-сервис по имени.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса.</param>
        /// <exception cref="FileNotFoundException">Выбрасывается, если файл API-сервиса не найден для удаления.</exception>
        /// <exception cref="IOException">Выбрасывается, если произошла ошибка при удалении файла.</exception>
        /// <exception cref="UnauthorizedAccessException">Выбрасывается, если нет доступа для удаления файла.</exception>
        /// <exception cref="PathTooLongException">Выбрасывается, если путь к файлу слишком длинный при удалении.</exception>   
        public void DeleteApiService(string apiServiceName)
        {
            string filePath = GetFilePath(apiServiceName);

            var semapthore = GetSemaphore(filePath);

            semapthore.WaitAsync();
            try
            {
                File.Delete(filePath);

                lock (_lock)
                    _fileSemaphores.TryRemove(apiServiceName, out _);
            }
            catch (FileNotFoundException ex)
            {
                throw new FileNotFoundException("Файл API-сервиса не найден для удаления.", ex);
            }
            catch (IOException ex)
            {
                throw new IOException("Ошибка при удалении API-сервиса.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("Нет доступа для удаления файла API-сервиса.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Произошла непредвиденная ошибка.", ex);
            }
            finally
            {
                semapthore.Release();
            }
        }
        public async Task<List<ApiServiceDto>> GetAllServicesAsync()
        {
            var apiServiceNames = GetApiServiceNames();
            List<ApiServiceDto> apiServices = new List<ApiServiceDto>();
            ApiService apiService;
            foreach (var apiServiceName in apiServiceNames)
            {
                apiService = await DeserializeApiServiceAsync(apiServiceName);
                apiServices.Add(new ApiServiceDto
                {
                    Name = apiServiceName,
                    Description = apiService.Description,
                    IsActive = apiService.IsActive,
                    Entities = apiService.Entities,
                });
            }
            return apiServices;
        }
        /// <summary>
        /// Проверяет, существует ли API-сервис с указанным именем.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса.</param>
        /// <returns>True, если сервис существует; иначе false.</returns>
        public bool IsApiServiceExist(string apiServiceName)
        {
            return File.Exists(GetFilePath(apiServiceName));
        }
    }
}

