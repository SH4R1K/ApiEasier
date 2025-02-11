namespace ApiEasier.Bll.Interfaces.Cache
{
    public interface ICacheService
    {
        Task<T?> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan cacheDuration);
        void Remove(string key);
    }
}
