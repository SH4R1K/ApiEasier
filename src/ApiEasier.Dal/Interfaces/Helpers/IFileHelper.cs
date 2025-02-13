namespace ApiEasier.Dal.Interfaces.Helpers
{
    /// <summary>
    /// Обеспечивает работу с файлами
    /// </summary>
    public interface IFileHelper
    {
        /// <summary>
        /// Получает названия всех файлов
        /// </summary>
        /// <returns>Список названий файлов</returns>
        public Task<List<string>> GetAllFileNamesAsync();

        /// <summary>
        /// Читает содержимое файла
        /// </summary>
        /// <typeparam name="T">Тип получаемого содержимого файла</typeparam>
        /// <param name="fileName">Имя файла без расширения</param>
        /// <returns>Содержимое файла</returns>
        public Task<T?> ReadAsync<T>(string fileName);
        public Task<T?> WriteAsync<T>(string fileName, T data);
        public bool Delete(string fileName);
    }
}
