using ApiEasier.Dal.Interfaces.Helpers;

namespace ApiEasier.Dal.Helpers
{
    public class FileHelper : IFileHelper
    {
        public required string _folderPath;

        public FileHelper(string folderPath)
        {
            _folderPath = folderPath ?? throw new ArgumentException(nameof(folderPath));
        }

        private string GetFilePath(string fileName) => Path.Combine(_folderPath, fileName + ".json");

        public List<string?> GetAllFiles()
        {
            return Directory.GetFiles(_folderPath, "*.json")
                            .Select(Path.GetFileName)
                            .ToList();
        }

        public async Task<T?> ReadJsonAsync<T>(string fileName)
        {
            var filePath = GetFilePath(fileName);

            if (!File.Exists(filePath))
                return default;

            var json = await File.ReadAllTextAsync(filePath);
            return JsonHelper.Deserialize<T>(json);
        }

        public async Task WriteJsonAsync<T>(string fileName, T data)
        {
            var filePath = GetFilePath(fileName);
            var json = JsonHelper.Serialize(data);
            await File.WriteAllTextAsync(filePath, json);
        }

        public bool DeleteFile(string fileName)
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
