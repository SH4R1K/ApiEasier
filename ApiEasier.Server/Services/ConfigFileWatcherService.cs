using Microsoft.Extensions.Caching.Memory;

namespace ApiEasier.Server.Services
{
    /// <summary>
    /// Сервис для отслеживания изменений в конфигурационных файлах.
    /// </summary>
    public class ConfigFileWatcherService : IHostedService
    {
        private FileSystemWatcher _fileSystemWatcher;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ConfigFileWatcherService"/>.
        /// </summary>
        /// <param name="path">Путь к директории, в которой будут отслеживаться изменения файлов.</param>
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

        /// <summary>
        /// Запускает отслеживание изменений в файлах.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены для управления жизненным циклом сервиса.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _fileSystemWatcher.EnableRaisingEvents = true;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Останавливает отслеживание изменений в файлах.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены для управления жизненным циклом сервиса.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
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
