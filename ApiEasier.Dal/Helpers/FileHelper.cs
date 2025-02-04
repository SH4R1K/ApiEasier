namespace ApiEasier.Dal.Helpers
{
    public class FileHelper
    {
        private readonly string _folderPath;

        public FileHelper(string folderPath)
        {
            _folderPath = folderPath;
        }

        public string GetFilePath(string fileName)
        {
            return Path.Combine(_folderPath, fileName + ".json");
        }

        public List<string?> GetAllFiles()
        {
            return Directory.GetFiles(_folderPath, "*.json")
                            .Select(Path.GetFileName)
                            .ToList();
        }

        public async Task<string?> ReadFileAsync(string filePath)
        {
            return File.Exists(filePath) ? await File.ReadAllTextAsync(filePath) : null;
        }

        public async Task WriteFileAsync(string filePath, string content)
        {
            await File.WriteAllTextAsync(filePath, content);
        }
    }
}
