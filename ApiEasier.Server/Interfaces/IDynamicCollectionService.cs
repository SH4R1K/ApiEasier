using MongoDB.Bson;

namespace ApiEasier.Server.Interfaces
{
    public interface IDynamicCollectionService
    {
        Task<Dictionary<string, object>?> AddDocToCollectionAsync(string collectionName, object jsonData);

        Task<List<Dictionary<string, object>?>> GetDocFromCollectionAsync(string collectionName);

        Task<Dictionary<string, object>?> GetDocByIdFromCollectionAsync(string collectionName, string id);

        Task<Dictionary<string, object>?> UpdateDocFromCollectionAsync(string collectionName, object jsonData);

        Task<long?> DeleteDocFromCollectionAsync(string collectionName, string id);
    }
}
