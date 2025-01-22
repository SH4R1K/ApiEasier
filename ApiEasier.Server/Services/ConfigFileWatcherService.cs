using ApiEasier.Server.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ApiEasier.Server.Services
{
    public class ConfigFileWatcherService : IHostedService
    {
        private FileSystemWatcher _fileSystemWatcher;
        private readonly IMemoryCache _cache;


        public ConfigFileWatcherService(string path, IMemoryCache cache)
        {
            _fileSystemWatcher = new FileSystemWatcher(path)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.json"
            };

            _fileSystemWatcher.Changed += OnChanged;
            _fileSystemWatcher.Deleted += OnChanged;
            _fileSystemWatcher.Renamed += OnRenamed;
            _cache = cache;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _fileSystemWatcher.EnableRaisingEvents = true;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
            return Task.CompletedTask;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            //удаление кэша
            _cache.Remove(Path.GetFileNameWithoutExtension(e.Name));
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            //удаление кэша по старому имени
            _cache.Remove(Path.GetFileNameWithoutExtension(e.OldName));
        }
    }
}
