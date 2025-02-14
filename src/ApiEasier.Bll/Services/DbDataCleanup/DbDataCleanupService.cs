using ApiEasier.Bll.Interfaces.DbDataCleanup;
using ApiEasier.Bll.Interfaces.Logger;
using ApiEasier.Dal.Interfaces;

namespace ApiEasier.Bll.Services.DbDataCleanup
{
    public class DbDataCleanupService : IDbDataCleanupService
    {
        private readonly ILoggerService _loggerService;
        private readonly IResourceRepository _dbResourceRepository;
        private readonly IApiServiceRepository _fileApiServiceRepository;


        public DbDataCleanupService(
            IResourceRepository dbResourceRepository,
            IApiServiceRepository fileApiServiceRepository,
            ILoggerService loggerService)
        {
            _fileApiServiceRepository = fileApiServiceRepository;
            _dbResourceRepository = dbResourceRepository;
            _loggerService = loggerService;
        }

        public async Task CleanupAsync()
        {
            try
            {
                var apiServices = await _fileApiServiceRepository.GetAllAsync();

                List<string> apiServiceNames = apiServices.Select(a => a.Name).ToList();

                await _dbResourceRepository.DeleteUnusedResources(apiServiceNames);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Чистка БД от неиспользуемых хранилищ вызвала исключение");
            }

        }
    }
}
