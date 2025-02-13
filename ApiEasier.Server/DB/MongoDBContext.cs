﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;

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
        public MongoDbContext(IOptions<DbSettings> settings)
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
        public async Task DropCollectionAsync(string name)
        {
            await _database.DropCollectionAsync(name);
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
