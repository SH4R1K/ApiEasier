namespace ApiEasier.Server.Interfaces
{
    public interface IDynamicCollectionService
    {
        Task AddToCollectionAsync(string collectionName, object jsonData);
    }
}
