using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Dal.Helpers;
using ApiEasier.Dal.Interfaces.Db;

namespace ApiEasier.Bll.Services.ApiEmu
{
    // TODO: так же возможно сменить тип возврата
    public class DynamicResource : IDynamicResource
    {
        private readonly IDbResourceDataRepository _dbResourceDataRepository;

        public DynamicResource(IDbResourceDataRepository dbResourceDataRepository)
        {
            _dbResourceDataRepository = dbResourceDataRepository;
        }

        public async Task<DynamicResourceModel> AddDataAsync(string apiName, string apiEntityName, object jsonData)
        {
            try
            {
                string resourceName = apiName.Trim().Replace(" ", "") + "_" + apiEntityName.Trim().Replace(" ", "");

                var result = await _dbResourceDataRepository.CreateDataAsync(resourceName, jsonData);
                return result ?? throw new InvalidOperationException("Не удалось добавить данные");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                throw new ArgumentException("Ошибка при добавлении данных: " + ex.Message, ex);
            }
        }

        public async Task<DynamicResourceDataModel> GetDataByIdAsync(string apiName, string apiEntityName, string id, string? filters)
        {
            try
            {
                string resourceName = apiName.Trim().Replace(" ", "") + "_" + apiEntityName.Trim().Replace(" ", "");

                var data = await _dbResourceDataRepository.GetDataByIdAsync(resourceName, id);

                //var collections = await _dbContext.GetListCollectionNamesAsync();
                //if (!collections.Contains(collectionName))
                //    return null;

                //var collection = _dbContext.GetCollection<BsonDocument>(collectionName);
                //if (!ObjectId.TryParse(id, out var objectId))
                //    return null;

                //FilterDefinition<BsonDocument> documentFilter;
                //try
                //{
                //    documentFilter = BsonSerializer.Deserialize<BsonDocument>(filters);
                //}
                //catch
                //{
                //    documentFilter = Builders<BsonDocument>.Filter.Empty;
                //}

                //var idFilter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
                //var combineFilter = Builders<BsonDocument>.Filter.And(idFilter, documentFilter);
                //var document = await collection.Find(combineFilter).FirstOrDefaultAsync();
                //if (document == null)
                //    return null;

                //var bsonDoc = document.ToDictionary();
                //bsonDoc["_id"] = document["_id"].ToString();


                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                throw new ArgumentException("Ошибка при получении документа: " + ex.Message, ex);
            }
        }


        public async Task<List<DynamicResourceModel>?> GetDataAsync(string apiName, string apiEntityName, string? filters)
        {
            try
            {
                string resourceName = apiName.Trim().Replace(" ", "") + "_" + apiEntityName.Trim().Replace(" ", "");

                var data = await _dbResourceDataRepository.GetAllDataAsync(resourceName);

                //var collection = _dbContext.GetCollection<BsonDocument>(collectionName);
                //FilterDefinition<BsonDocument> filterDefinition;

                //try
                //{
                //    filterDefinition = BsonSerializer.Deserialize<BsonDocument>(filters);
                //}
                //catch
                //{
                //    filterDefinition = Builders<BsonDocument>.Filter.Empty;
                //}

                //var documents = await collection.Find(filterDefinition).ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                throw new ArgumentException("Ошибка при получении данных: " + ex.Message, ex);
            }
        }

        public async Task<bool> DeleteDataAsync(string apiName, string apiEntityName, string id)
        {
            string resourceName = apiName.Trim().Replace(" ", "") + "_" + apiEntityName.Trim().Replace(" ", "");

            var result = await _dbResourceDataRepository.DeleteDataAsync(resourceName, id);

            return result;
        }

        public async Task<DynamicResourceModel> UpdateDataAsync(string apiName, string apiEntityName, string id, object jsonData)
        {
            string resourceName = apiName.Trim().Replace(" ", "") + "_" + apiEntityName.Trim().Replace(" ", "");

            var result = await _dbResourceDataRepository.UpdateDataAsync(resourceName, id, jsonData);

            return result;
        }

        /// <summary>
        /// Удаляет коллекции по удалении конфигурации API-сервиса.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса</param>
        //public async Task DeleteCollectionsByDeletedApiServiceAsync(string apiServiceName)
        //{
        //    foreach (var collection in (await _dbContext.GetListCollectionNamesAsync()).Where(c => c.Split("_")[0] == apiServiceName))
        //    {
        //        await _dbContext.DropCollectionAsync(collection);
        //    }
        //}

        /// <summary>
        /// Удаляет коллекцию по изменении конфигурации API-сервиса.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса</param>
        //public async Task DeleteCollectionsByChangedApiServiceAsync(string apiServiceName)
        //{
        //    var apiService = await _configFileApiService.DeserializeApiServiceAsync(apiServiceName);
        //    foreach (var collection in (await _dbContext.GetListCollectionNamesAsync()).Where(c => c.Split("_")[0] == apiServiceName && !apiService.Entities.Any(e => e.Name == c.Split("_")[1])))
        //    {
        //        await _dbContext.DropCollectionAsync(collection);
        //    }
        //}

        /// <summary>
        /// Переименовывает коллекцию по новому имени API-сервиса.
        /// </summary>
        /// <param name="oldApiServiceName">Старое имя API-сервиса</param>
        /// <param name="apiServiceName">Новое имя API-сервиса</param>
        //public async Task RenameCollectionsByApiServiceAsync(string oldApiServiceName, string apiServiceName)
        //{
        //    var list = await _dbContext.GetListCollectionNamesAsync();
        //    var temp = list.FirstOrDefault().Split("_")[0];
        //    foreach (var collection in (await _dbContext.GetListCollectionNamesAsync()).Where(c => c.Split("_")[0] == oldApiServiceName))
        //    {
        //        await _dbContext.RenameCollectionAsync(collection, $"{apiServiceName}_{collection.Split("_")[1]}");
        //    }
        //}

        /// <summary>
        /// Находит неиспользуемые коллекции по всем файлам конфигурации и удаляет их
        /// </summary>
        //public async Task DeleteTrashCollectionAsync()
        //{
        //    var apiServices = await _configFileApiService.GetApiServicesAsync();
        //    foreach (var collection in (await _dbContext.GetListCollectionNamesAsync())
        //        .Where(c => !apiServices.Any(asn => c.Split("_")[0] == asn.Name) || !apiServices.Any(ase => ase.Entities.Any(e => e.Name == c.Split("_")[1]))))
        //        await _dbContext.DropCollectionAsync(collection);
        //}
    }
}

