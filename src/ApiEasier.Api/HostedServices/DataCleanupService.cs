using ApiEasier.Bll.Interfaces.DbDataCleanup;
using ApiEasier.Logger.Interfaces;

namespace ApiEasier.Api.HostedServices
{
    public class DataCleanupService : IHostedService
    {
        private readonly ILoggerService _loggerService;

        private readonly IDbDataCleanupService _dbDataCleanupService;

        public DataCleanupService(IDbDataCleanupService dbDataCleanupService, ILoggerService loggerService)
        {
            _dbDataCleanupService = dbDataCleanupService;
            _loggerService = loggerService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _loggerService.LogDebug("Чистка БД от неиспользуемых хранилищ");
            await _dbDataCleanupService.CleanupAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
