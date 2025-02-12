using ApiEasier.Bll.Interfaces.FileWatcher;
using ApiEasier.Dal.Interfaces.Db;

namespace ApiEasier.Bll.Services.FileWatcher
{
    // Можно пихнуть cache сюда
    public class ApiConfigChangeHandler : IApiConfigChangeHandler
    {
        private readonly IDbResourceRepository _dbResourceRepository;

        public ApiConfigChangeHandler(IDbResourceRepository dbResourceRepository)
        {
            _dbResourceRepository = dbResourceRepository;
        }

        public async Task OnConfigDeletedAsync(string configName)
        {
            await _dbResourceRepository.DeleteAsync(configName);
        }
    }
}
