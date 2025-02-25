using ApiEasier.Bll.Interfaces.DbDataCleanup;
using ApiEasier.Logger.Interfaces;
using ApiEasier.Dal.Interfaces;

namespace ApiEasier.Bll.Services.DbDataCleanup
{
    /// <inheritdoc cref="IResourcesCleanupService"/>
    public class DataCleanupService : IResourcesCleanupService
    {
        private readonly ILoggerService _loggerService;
        private readonly IResourceRepository _resourceRepository;
        private readonly IApiServiceRepository _apiServiceRepository;


        public DataCleanupService(
            IResourceRepository resourceRepository,
            IApiServiceRepository apiServiceRepository,
            ILoggerService loggerService)
        {
            _apiServiceRepository = apiServiceRepository;
            _resourceRepository = resourceRepository;
            _loggerService = loggerService;
        }

        public async Task CleanupAsync()
        {
            try
            {
                var apiServices = await _apiServiceRepository.GetAllAsync();

                List<string> apiServiceNames = apiServices.Select(a => a.Name).ToList();

                await _resourceRepository.DeleteUnusedResources(apiServiceNames);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Чистка БД от неиспользуемых хранилищ вызвала исключение {ex}");
            }

        }
    }
}
