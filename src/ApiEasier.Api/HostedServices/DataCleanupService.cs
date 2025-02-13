using ApiEasier.Bll.Interfaces.DbDataCleanup;

namespace ApiEasier.Api.HostedServices
{
    public class DataCleanupService : IHostedService
    {
        private readonly IDbDataCleanupService _dbDataCleanupService;

        public DataCleanupService(IDbDataCleanupService dbDataCleanupService)
        {
            _dbDataCleanupService = dbDataCleanupService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Чистка БД от неиспользуемых хранилищ");
            await _dbDataCleanupService.CleanupAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
