using ApiEasier.Server.Dto;
using ApiEasier.Server.Models;
using System.Text.Json;

namespace ApiEasier.Server.Services
{
    public class JsonService
    {
        private readonly string _path;
        public JsonService(string path)
        {
            _path = path;
            Directory.CreateDirectory(_path);
        }
        private string GetFilePath(string fileName)
        {
            string filePath = Path.Combine(_path, fileName + ".json");

            return filePath;
        }

        public async Task<ApiService?> DeserializeApiServiceAsync(string apiServiceName)
        {
            var filePath = GetFilePath(apiServiceName);

            if (!File.Exists(filePath))
            {
                return null;
            }

            var json = await File.ReadAllTextAsync(filePath);

            // Десериализация JSON в объект
            var apiService = JsonSerializer.Deserialize<ApiService>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Уменьшение регистра полей
                WriteIndented = true // Запись в читаемом формате
            });
            return apiService;
        }

        public async Task SerializeApiServiceAsync(string apiServiceName, ApiService apiService)
        {
            var filePath = GetFilePath(apiServiceName);

            if (!File.Exists(filePath))
            {
                throw new Exception($"Файл не существует");
            }

            // Десериализация JSON в объект
            var json = JsonSerializer.Serialize(apiService, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Уменьшение регистра полей
                WriteIndented = true // Запись в читаемом формате
            });

            await File.WriteAllTextAsync(filePath, json);
        }


        public async Task<ApiEntity?> GetApiEntity(string entityName, string apiServiceName)
        {
            var filePath = GetFilePath(apiServiceName);
            var apiService = await DeserializeApiServiceAsync(filePath);
            if (apiService == null)
                return null;
            var entity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);

            return entity;
        }

        public IEnumerable<string> GetApiServiceNames()
        {
            DirectoryInfo directory = new DirectoryInfo("configuration");
            var files = directory.GetFiles();
            return files.Select(f => Path.GetFileNameWithoutExtension(f.Name));
        }

        public async Task<ApiServiceDto?> GetApiServiceByName(string apiServiceName)
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

        public void RenameApiService(string oldName, ApiServiceDto apiServiceDto)
        {
            if (oldName != apiServiceDto.Name)
            {
                // Определение старого и нового путей к файлу
                string oldFilePath = GetFilePath(oldName);
                string newFilePath = GetFilePath(apiServiceDto.Name);

                // Переименование файла
                File.Move(oldFilePath, newFilePath);
            }
        }
        public void DeleteApiService(string apiServiceName)
        {
            string filePath = GetFilePath(apiServiceName);
            if (!File.Exists(filePath))
            {
                throw new Exception($"Файл не существует");
            }
            File.Delete(filePath);
        }
    }
}
