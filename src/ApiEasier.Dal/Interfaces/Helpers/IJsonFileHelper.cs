namespace ApiEasier.Dal.Interfaces.Helpers
{
    /// <summary>
    /// Обеспечивает работу с файлами JSON
    /// </summary>
    public interface IJsonFileHelper : IFileHelper
    {
        /// <inheritdoc cref="IFileHelper.GetAllFileNamesAsync"/>
        public new Task<List<string>> GetAllFileNamesAsync();

        /// <inheritdoc cref="IFileHelper.ReadAsync"/>
        public new Task<T?> ReadAsync<T>(string fileName);
        public new Task<T?> WriteAsync<T>(string fileName, T data);
        public new bool Delete(string fileName);
    }
}
