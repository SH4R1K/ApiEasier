using MongoDB.Bson;

namespace ApiEasier.Server.Interfaces
{
    public interface IDynamicCollectionService
    {
        Task AddDocToCollectionAsync(string collectionName, object jsonData);

        Task<List<Dictionary<string, object>>> GetDocFromCollectionAsync(string collectionName);
    }
}
