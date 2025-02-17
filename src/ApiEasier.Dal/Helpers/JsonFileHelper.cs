using ApiEasier.Dal.Interfaces.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace ApiEasier.Dal.Helpers
{
    /// <summary>
    /// Обеспечивает работу с файлами JSON
    /// </summary>
    public class JsonFileHelper : IFileHelper
    {
        private readonly IMemoryCache _cache;
        private static readonly SemaphoreSlim _lock = new(1, 1);

        public string FolderPath { get; init; }

        public JsonFileHelper(string folderPath, IMemoryCache cache)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new ArgumentException("Путь к папке не может быть пустым.", nameof(folderPath));

            FolderPath = folderPath;
            _cache = cache;

            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }
        }

        /// <summary>
        /// Возвращает полный путь к файлу JSON по его имени без расширения
        /// </summary>
        /// <param name="fileName">Имя файла JSON без расширения</param>
        /// <returns>Полный путь к файлу JSON</returns>
        private string GetFilePath(string fileName) => Path.Combine(FolderPath, fileName + ".json");

        /// <summary>
        /// Получает список названий файлов JSON без расширения из папки для конфигураций
        /// </summary>
        /// <inheritdoc/>
        public async Task<List<string>> GetAllFileNamesAsync()
        {
            try
            {
                return await Task.Run(() =>
                Directory.GetFiles(FolderPath, "*.json")
                .Select(f => Path.GetFileNameWithoutExtension(f)!)
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList());
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(FolderPath);

                return await GetAllFileNamesAsync();
            }
        }

        /// <summary>
        /// Читает и сериализует содержимое файла JSON (для ускорения данные кэшируется и, если кэш есть, то файл не читается, а данные берутся из кэша)
        /// </summary>
        /// <typeparam name="T">Тип объекта получаемого из содержимого файла JSON</typeparam>
        /// <returns>Объект из JSON</returns>
        /// <inheritdoc/>
        public async Task<T?> ReadAsync<T>(string fileName)
        {
            if (_cache.TryGetValue(fileName, out T? value))
            {
                return value;
            }

            var filePath = GetFilePath(fileName);

            if (!File.Exists(filePath))
                return default;

            await _lock.WaitAsync();
            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                var data = JsonSerializerHelper.Deserialize<T>(json);

                _cache.Set(fileName, data, TimeSpan.FromHours(1));

                return data;
            }
            finally
            {
                _lock.Release();
            }
        }

        /// <summary>
        /// Сериализует данные в JSON и записывает их в файл
        /// </summary>
        /// <inheritdoc/>
        public async Task<T?> WriteAsync<T>(string fileName, T data)
        {
            //на случай отказа от FileSystemWatcherService
            //_cache.Remove(fileName);

            var filePath = GetFilePath(fileName);

            if (File.Exists(filePath))
                return default;

            await _lock.WaitAsync();
            try
            {
                var json = JsonSerializerHelper.Serialize(data);
                await File.WriteAllTextAsync(filePath, json);

                return data;
            }
            finally
            {
                _lock?.Release();
            }
        }

        public bool Delete(string fileName)
        {
            try
            {
                _lock.Wait();
                var filePath = GetFilePath(fileName);
                if (!File.Exists(filePath))
                    Directory.CreateDirectory(FolderPath);

                File.Delete(filePath);
                return true;
            }
            catch
            {
                Console.WriteLine($"Ошибка при удалении файла {fileName}");
                return false;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
