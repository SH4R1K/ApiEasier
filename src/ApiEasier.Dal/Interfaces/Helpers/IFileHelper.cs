namespace ApiEasier.Dal.Interfaces.Helpers
{
    public interface IFileHelper
    {
        public Task<List<string>> GetAllFilesAsync();
        public Task<T?> ReadAsync<T>(string fileName);
        public Task<T?> WriteAsync<T>(string fileName, T data);
        public bool Delete(string fileName);
    }
}
