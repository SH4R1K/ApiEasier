using ApiEasier.Dal.Interfaces.Helpers;

namespace ApiEasier.Dal.Helpers
{
    /// <summary>
    /// Класс для работы с файлами json
    /// </summary>
    public class JsonFileHelper
    {
        public string FolderPath { get; init; }

        public JsonFileHelper(string folderPath)
        {
            FolderPath = folderPath ?? throw new ArgumentException(nameof(folderPath));
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
            var filePath = GetFilePath(fileName);

            if (!File.Exists(filePath))
                return default;

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializerHelper.Deserialize<T>(json);
        }

        public async Task WriteAsync<T>(string fileName, T data)
        {
            var filePath = GetFilePath(fileName);
            var json = JsonSerializerHelper.Serialize(data);
            await File.WriteAllTextAsync(filePath, json);
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
