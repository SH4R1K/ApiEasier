using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Server.Db;
using Microsoft.Extensions.Caching.Memory;

namespace ApiEasier.Bll.Services
{
    /// <summary>
    /// Сервис для отслеживания изменений в конфигурационных файлах.
    /// </summary>
    public class ConfigFileWatcherService : IHostedService
    {
        private FileSystemWatcher _fileSystemWatcher;
        private readonly IMemoryCache _cache;
        private readonly IDynamicResource _dynamicCollectionService;
        private readonly IConfiguration _config;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ConfigFileWatcherService"/>.
        /// </summary>
        public ConfigFileWatcherService(IMemoryCache cache, IDynamicResource dynamicCollectionService, IConfiguration config)
        {
            _config = config;
            _fileSystemWatcher = new FileSystemWatcher(_config["JsonDirectoryPath"])
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.json"
            };

            _fileSystemWatcher.Changed += OnChanged;
            _fileSystemWatcher.Deleted += OnDeleted;
            _fileSystemWatcher.Renamed += OnRenamed;
            _cache = cache;
            _dynamicCollectionService = dynamicCollectionService;
        }

        /// <summary>
        /// Запускает отслеживание изменений в файлах.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены для управления жизненным циклом сервиса.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Останавливает отслеживание изменений в файлах.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены для управления жизненным циклом сервиса.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
        }

        private async void OnChanged(object sender, FileSystemEventArgs e)
        {
            var apiServiceName = Path.GetFileNameWithoutExtension(e.Name);
            //удаление кэша
            _cache.Remove(Path.GetFileNameWithoutExtension(apiServiceName));

            await _dynamicCollectionService.DeleteCollectionsByChangedApiServiceAsync(apiServiceName);
        }

        private async void OnDeleted(object sender, FileSystemEventArgs e)
        {
            var apiServiceName = Path.GetFileNameWithoutExtension(e.Name);
            //удаление кэша
            _cache.Remove(Path.GetFileNameWithoutExtension(apiServiceName));

            await _dynamicCollectionService.DeleteCollectionsByDeletedApiServiceAsync(apiServiceName);
        }

        private async void OnRenamed(object sender, RenamedEventArgs e)
        {
            var oldApiServiceName = Path.GetFileNameWithoutExtension(e.OldName);
            var apiServiceName = Path.GetFileNameWithoutExtension(e.Name);
            //удаление кэша по старому имени
            _cache.Remove(Path.GetFileNameWithoutExtension(oldApiServiceName));

            await _dynamicCollectionService.RenameCollectionsByApiServiceAsync(oldApiServiceName, apiServiceName);
        }
    }
}
