using ApiEasier.Server.Dto;
using ApiEasier.Server.Interfaces;
using ApiEasier.Server.Models;
using System.Text.Json;

namespace ApiEasier.Server.Services
{
    /// <summary>
    /// Сервис для получения и редактирования информации про API-сервисы.
    /// </summary>
    public class JsonService : IConfigFileApiService
    {
        private readonly string _path;

        /// <summary>
        /// Базовый конструктор JsonService.
        /// </summary>
        /// <param name="path">Путь к папке конфигураций.</param>
        /// <exception cref="InvalidOperationException">Выбрасывается, если не удается создать директорию.</exception>
        public JsonService(string path)
        {
            try
            {
                _path = path;
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
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(_path, fileName + ".json");
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
            {
                return null;
            }

            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<ApiService>(json, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });
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
            var json = JsonSerializer.Serialize(apiService, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            try
            {
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
            DirectoryInfo directory = new DirectoryInfo(_path);
            var files = directory.GetFiles("*.json");
            return files.Select(f => Path.GetFileNameWithoutExtension(f.Name));
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
                string oldFilePath = GetFilePath(oldName);
                string newFilePath = GetFilePath(apiServiceDto.Name);

                try
                {
                    File.Move(oldFilePath, newFilePath);
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

            try
            {
                File.Delete(filePath);
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

