using ApiEasier.Server.DB;
using ApiEasier.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEasier.Server.Services
{
    /// <summary>
    /// Сервис для управления динамическими коллекциями в MongoDB.
    /// </summary>
    public class DynamicCollectionService : IDynamicCollectionService
    {
        private readonly MongoDBContext _dbContext;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DynamicCollectionService"/>.
        /// </summary>
        /// <param name="dbContext">Контекст MongoDB.</param>
        public DynamicCollectionService(MongoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Добавляет документ в указанную коллекцию.
        /// </summary>
        /// <param name="collectionName">Имя коллекции.</param>
        /// <param name="jsonData">Данные в формате JSON для добавления.</param>
        /// <returns>Словарь, представляющий добавленный документ.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, когда происходит ошибка во время операции.</exception>
        public async Task<Dictionary<string, object>> AddDocToCollectionAsync(string collectionName, object jsonData)
        {
            try
            {
                var collection = _dbContext.GetCollection<BsonDocument>(collectionName);
                var bsonDocument = BsonDocument.Parse(jsonData.ToString());
                await collection.InsertOneAsync(bsonDocument);

                var bsonDoc = bsonDocument.ToDictionary();
                bsonDoc["_id"] = bsonDocument["_id"].ToString();
                return bsonDoc;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                throw new ArgumentException("Ошибка при добавлении документа: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Удаляет документ из указанной коллекции по его ID.
        /// </summary>
        /// <param name="collectionName">Имя коллекции.</param>
        /// <param name="id">ID документа для удаления.</param>
        /// <returns>Количество удаленных документов.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, когда происходит ошибка во время операции.</exception>
        public async Task<long?> DeleteDocFromCollectionAsync(string collectionName, string id)
        {
            try
            {
                var collections = await _dbContext.ListCollectionNamesAsync();
                if (!collections.Contains(collectionName))
                    return null;

                var collection = _dbContext.GetCollection<BsonDocument>(collectionName);
                if (!ObjectId.TryParse(id, out var objectId))
                    return null;

                var result = await collection.DeleteOneAsync(Builders<BsonDocument>.Filter.Eq("_id", objectId));
                return result.DeletedCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                throw new ArgumentException("Ошибка при удалении документа: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Получает документ из указанной коллекции по его ID.
        /// </summary>
        /// <param name="collectionName">Имя коллекции.</param>
        /// <param name="id">ID документа для получения.</param>
        /// <param name="filters">Дополнительные фильтры для применения.</param>
        /// <returns>Словарь, представляющий документ, или null, если не найден.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, когда происходит ошибка во время операции.</exception>
        public async Task<Dictionary<string, object>?> GetDocByIdFromCollectionAsync(string collectionName, string id, string? filters)
        {
            try
            {
                var collections = await _dbContext.ListCollectionNamesAsync();
                if (!collections.Contains(collectionName))
                    return null;

                var collection = _dbContext.GetCollection<BsonDocument>(collectionName);
                if (!ObjectId.TryParse(id, out var objectId))
                    return null;

                FilterDefinition<BsonDocument> documentFilter;
                try
                {
                    documentFilter = BsonSerializer.Deserialize<BsonDocument>(filters);
                }
                catch
                {
                    documentFilter = Builders<BsonDocument>.Filter.Empty;
                }

                var idFilter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
                var combineFilter = Builders<BsonDocument>.Filter.And(idFilter, documentFilter);
                var document = await collection.Find(combineFilter).FirstOrDefaultAsync();
                if (document == null)
                    return null;

                var bsonDoc = document.ToDictionary();
                bsonDoc["_id"] = document["_id"].ToString();
                return bsonDoc;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                throw new ArgumentException("Ошибка при получении документа: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Получает документы из указанной коллекции с применением фильтров.
        /// </summary>
        /// <param name="collectionName">Имя коллекции.</param>
        /// <param name="filters">Фильтры для получения документов.</param>
        /// <returns>Список словарей, представляющих документы, или null, если не найдено.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, когда происходит ошибка во время операции.</exception>
        public async Task<List<Dictionary<string, object>>?> GetDocFromCollectionAsync(string collectionName, string? filters)
        {
            try
            {
                var collection = _dbContext.GetCollection<BsonDocument>(collectionName);
                FilterDefinition<BsonDocument> filterDefinition;

                try
                {
                    filterDefinition = BsonSerializer.Deserialize<BsonDocument>(filters);
                }
                catch
                {
                    filterDefinition = Builders<BsonDocument>.Filter.Empty;
                }

                var documents = await collection.Find(filterDefinition).ToListAsync();
                var jsonList = documents.Select(doc =>
                {
                    var bsonDoc = doc.ToDictionary();
                    bsonDoc["_id"] = doc["_id"].ToString(); // Преобразуем ObjectId в строку для корректного отображения
                    return bsonDoc;
                }).ToList();

                return jsonList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                throw new ArgumentException("Ошибка при получении документов: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Обновляет документ в указанной коллекции по его ID.
        /// </summary>
        /// <param name="collectionName">Имя коллекции.</param>
        /// <param name="id">ID документа для обновления.</param>
        /// <param name="jsonData">Данные в формате JSON для обновления.</param>
        /// <returns>Словарь, представляющий обновленный документ, или null, если документ не найден.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, когда происходит ошибка во время операции.</exception>
        public async Task<Dictionary<string, object>?> UpdateDocFromCollectionAsync(string collectionName, string id, object jsonData)
        {
            try
            {
                var collections = await _dbContext.ListCollectionNamesAsync();
                if (!collections.Contains(collectionName))
                    return null;

                var collection = _dbContext.GetCollection<BsonDocument>(collectionName);
                if (!ObjectId.TryParse(id, out var objectId))
                    return null;

                var document = await collection.Find(Builders<BsonDocument>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();
                if (document == null)
                    return null;

                document.Remove("_id"); // Удаляем _id из данных, чтобы не перезаписать его

                // Используем ReplaceOne для замены всего документа
                var replacementResult = await collection.ReplaceOneAsync(
                    Builders<BsonDocument>.Filter.Eq("_id", objectId),
                    BsonDocument.Parse(jsonData.ToString())
                );

                if (replacementResult.ModifiedCount > 0)
                {
                    // Если документ заменен, возвращаем обновленный документ
                    var updatedDocument = await collection.Find(Builders<BsonDocument>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();
                    if (updatedDocument != null)
                    {
                        var bsonDoc = updatedDocument.ToDictionary();
                        bsonDoc["_id"] = updatedDocument["_id"].ToString();
                        return bsonDoc;
                    }
                }

                return null; // Если документ не был заменен
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                throw new ArgumentException("Ошибка при обновлении документа: " + ex.Message, ex);
            }
        }
    }
}

