using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Bll.Interfaces.FileWatcher;
using Microsoft.Extensions.Caching.Memory;

namespace ApiEasier.Bll.Services.FileWatcher
{
    /// <summary>
    /// Сервис для отслеживания изменений в конфигурационных файлах.
    /// </summary>
    public class ConfigFileWatcherService : IHostedService
    {
        private FileSystemWatcher _fileSystemWatcher;
        private readonly IApiConfigChangeHandler _apiConfigChangeHandler;
        private readonly IMemoryCache _cache;

        public ConfigFileWatcherService(
            IMemoryCache cache,
            string directoryPath,
            IApiConfigChangeHandler apiConfigChangeHandler)
        {
            _cache = cache;

            _fileSystemWatcher = new FileSystemWatcher(directoryPath)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.json",
                EnableRaisingEvents = false
            };

            _apiConfigChangeHandler = apiConfigChangeHandler;

            _fileSystemWatcher.Changed += OnChanged;

            _fileSystemWatcher.Deleted += OnDeleted;

            _fileSystemWatcher.Renamed += OnRenamed;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
             _fileSystemWatcher.EnableRaisingEvents = true;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
        }

        private async void OnChanged(object sender, FileSystemEventArgs e)
        {
            _cache.Remove(Path.GetFileNameWithoutExtension(e.Name));
        }

        private async void OnDeleted(object sender, FileSystemEventArgs e)
        {
            _cache.Remove(Path.GetFileNameWithoutExtension(e.Name));

            await _apiConfigChangeHandler.OnConfigDeletedAsync(Path.GetFileNameWithoutExtension(e.Name));
        }

        private async void OnRenamed(object sender, RenamedEventArgs e)
        {
            _cache.Remove(Path.GetFileNameWithoutExtension(e.OldName));
        }
    }
}
