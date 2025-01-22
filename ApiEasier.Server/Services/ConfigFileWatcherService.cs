using ApiEasier.Server.Interfaces;

namespace ApiEasier.Server.Services
{
    public class ConfigFileWatcherService : IHostedService
    {
        private FileSystemWatcher _fileSystemWatcher;

        public ConfigFileWatcherService(string path)
        {
            _fileSystemWatcher = new FileSystemWatcher(path)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.json"
            };

            _fileSystemWatcher.Changed += OnChanged;
            _fileSystemWatcher.Deleted += OnChanged;
            _fileSystemWatcher.Renamed += OnRenamed;
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
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            //удаление кэша по старому имени
        }
    }
}
