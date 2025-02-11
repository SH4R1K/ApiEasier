using ApiEasier.Bll.Interfaces.Cache;

namespace ApiEasier.Bll.Services.Cache
{
    public class CacheService : ICacheService
    {
        

        public Task<T?> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan cacheDuration)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }
    }
}
