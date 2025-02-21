using ApiEasier.Bll.Interfaces.DbDataCleanup;
using ApiEasier.Logger.Interfaces;

namespace ApiEasier.Api.HostedServices
{
    /// <summary>
    /// Удаляет при запуске неиспользуемые ресурсы хранения данных, на которых не ссылается ни одна сущность
    /// </summary>
    public class ResourcesCleanupService : IHostedService
    {
        private readonly ILoggerService _loggerService;

        private readonly IResourcesCleanupService _resourcesCleanupService;

        public ResourcesCleanupService(IResourcesCleanupService resourcesCleanupService, ILoggerService loggerService)
        {
            _resourcesCleanupService = resourcesCleanupService;
            _loggerService = loggerService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _loggerService.LogDebug("Чистка БД от неиспользуемых хранилищ");
            await _resourcesCleanupService.CleanupAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
