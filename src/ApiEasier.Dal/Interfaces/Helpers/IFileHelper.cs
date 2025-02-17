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
        /// Читает содержимое файла по имени
        /// </summary>
        /// <typeparam name="T">Тип получаемого содержимого файла</typeparam>
        /// <param name="fileName">Имя файла без расширения</param>
        /// <returns>Содержимое файла</returns>
        public Task<T?> ReadAsync<T>(string fileName);

        /// <summary>
        /// Записывает данные в файл
        /// </summary>
        /// <typeparam name="T">Тип записываемых данных</typeparam>
        /// <param name="fileName">Имя файла без расширения</param>
        /// <param name="data">Записываемые данные</param>
        /// <returns>Данные записавшиеся в файл</returns>
        public Task<T?> WriteAsync<T>(string fileName, T data);

        /// <summary>
        /// Удаляет файл по имени
        /// </summary>
        /// <param name="fileName">Имя удаляемого файла</param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(string fileName);
    }
}
