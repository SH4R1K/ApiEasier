using ApiEasier.Dal.Interfaces.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace ApiEasier.Dal.Helpers
{
    /// <summary>
    /// Класс для работы с файлами json
    /// </summary>
    public class JsonFileHelper : IJsonFileHelper
    {
        private readonly IMemoryCache _cache;

        public string FolderPath { get; init; }

        public JsonFileHelper(string folderPath, IMemoryCache cache)
        {
            FolderPath = folderPath ?? throw new ArgumentException(nameof(folderPath));
            _cache = cache;
        }

        private string GetFilePath(string fileName) => Path.Combine(FolderPath, fileName + ".json");

        public async Task<List<string>> GetAllFiles()
        {
            return await Task.Run(() =>
                Directory.GetFiles(FolderPath, "*.json")
                .Select(f => Path.GetFileNameWithoutExtension(f)!)
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList());
        }

        public async Task<T?> ReadAsync<T>(string fileName)
        {
            if(_cache.TryGetValue(fileName, out T? value))
            {
                return value;
            }

            var filePath = GetFilePath(fileName);

            if (!File.Exists(filePath))
                return default;

            var json = await File.ReadAllTextAsync(filePath);
            var data = JsonSerializerHelper.Deserialize<T>(json);

            _cache.Set(fileName, data, TimeSpan.FromHours(1));

            return data;
        }

        public async Task<T?> WriteAsync<T>(string fileName, T data)
        {
            //на случай отказа от FileSystemWatcherService
            //_cache.Remove(fileName);

            var filePath = GetFilePath(fileName);
            var json = JsonSerializerHelper.Serialize(data);
            await File.WriteAllTextAsync(filePath, json);

            return data;
        }

        public bool Delete(string fileName)
        {
            try
            {
                var filePath = GetFilePath(fileName);
                if (!File.Exists(filePath))
                    return false;

                File.Delete(filePath);
                return true;
            }
            catch
            {
                Console.WriteLine($"Ошибка при удалении файла {fileName}");
                return false;
            }
        }
    }
}
