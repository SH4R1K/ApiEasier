namespace ApiEasier.Dal.Interfaces.Helpers
{
    public interface IJsonFileHelper : IFileHelper
    {
        public new Task<List<string>> GetAllFiles();
        public new Task<T?> ReadAsync<T>(string fileName);
        public new Task<T?> WriteAsync<T>(string fileName, T data);
        public new bool Delete(string fileName);
    }
}
