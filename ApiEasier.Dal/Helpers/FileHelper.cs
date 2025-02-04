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

        public async Task<string?> ReadFileAsync(string fileName)
        {
            var filePath = GetFilePath(fileName);
            return File.Exists(filePath) ? await File.ReadAllTextAsync(filePath) : null;
        }

        public async Task WriteFileAsync(string fileName, string content)
        {
            var filePath = GetFilePath(fileName);
            await File.WriteAllTextAsync(filePath, content);
        }
    }
}
