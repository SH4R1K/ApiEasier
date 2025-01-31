using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections;

namespace ApiEasier.Dal.DB
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
        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
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
        public async Task<List<string>> GetListCollectionNamesAsync()
        {
            var collections = await _database.ListCollectionNamesAsync();
            return await collections.ToListAsync();
        }

        /// <summary>
        /// Асинхронно удаляет коллекцию по указанному имени.
        /// </summary>
        /// <param name="name">Имя коллекции для удаления.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        public async Task<bool> DropCollectionAsync(string name)
        {
            await _database.DropCollectionAsync(name);
            // Проверяем, осталась ли коллекция после удаления
            var filter = new BsonDocument("name", name);
            var collectionsCursor = await _database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });

            return !(await collectionsCursor.AnyAsync()); // true, если коллекции больше нет
        }

        /// <summary>
        /// Асинхронно создаёт коллекцию, если её нет.
        /// </summary>
        /// <param name="name">Имя коллекции.</param>
        /// <returns>True, если коллекция была создана, false, если уже существовала.</returns>
        public async Task<bool> CreateCollectionIfNotExistsAsync(string name)
        {
            var filter = new BsonDocument("name", name);
            var collectionsCursor = await _database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });

            if (await collectionsCursor.AnyAsync())
            {
                return false; // Коллекция уже существует
            }

            await _database.CreateCollectionAsync(name);
            return true; // Коллекция была создана
        }

        /// <summary>f
        /// Асинхронно переименовывает коллекцию.
        /// </summary>
        /// <param name="oldName">Старое имя коллекции.</param>
        /// <param name="newName">Новое имя коллекции.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        public async Task RenameCollectionAsync(string oldName, string newName)
        {
            await _database.RenameCollectionAsync(oldName, newName);
        }
    }
}
