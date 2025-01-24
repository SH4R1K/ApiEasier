
using ApiEasier.Server.Dto;
using ApiEasier.Server.Interfaces;
using ApiEasier.Server.Models;

namespace ApiEasier.Server.Services
{
    public class TakeOutDbTrashService : IHostedService
    {
        private readonly IDynamicCollectionService _dynamicCollectionService;

        public TakeOutDbTrashService(IDynamicCollectionService dynamicCollectionService)
        {
            _dynamicCollectionService = dynamicCollectionService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _dynamicCollectionService.DeleteTrashCollectionAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}
