using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiEasier.Server.Db
{
    /// <summary>
    /// Контекст для работы с MongoDB.
    /// </summary>
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MongoDbContext"/>.
        /// </summary>
        /// <param name="settings">Настройки базы данных, содержащие строку подключения и имя базы данных.</param>
        public MongoDbContext(IOptions<DbSerttings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        /// <summary>
        /// Получает коллекцию по указанному имени.
        /// </summary>
        /// <typeparam name="T">Тип документов в коллекции.</typeparam>
        /// <param name="name">Имя коллекции.</param>
        /// <returns>Коллекция документов указанного типа.</returns>
        public IMongoCollection<T> GetCollection<T>(string name) =>
            _database.GetCollection<T>(name);

        /// <summary>
        /// Асинхронно получает список имен коллекций в базе данных.
        /// </summary>
        /// <returns>Список имен коллекций.</returns>
        public async Task<List<string>> ListCollectionNamesAsync()
        {
            var collections = await _database.ListCollectionNamesAsync();
            return await collections.ToListAsync();
        }
    }
}
