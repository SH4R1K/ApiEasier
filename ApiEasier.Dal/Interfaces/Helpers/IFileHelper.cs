namespace ApiEasier.Dal.Interfaces.Helpers
{
    public interface IFileHelper
    {
        public List<string?> GetAllFiles();
        public Task<T?> ReadJsonAsync<T>(string fileName);
        public Task WriteJsonAsync<T>(string fileName, T data);
        public bool DeleteFile(string fileName);
    }
}
