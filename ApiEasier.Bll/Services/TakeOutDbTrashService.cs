using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Server.Dto;
using ApiEasier.Server.Models;

namespace ApiEasier.Bll.Services
{
    public class TakeOutDbTrashService : IHostedService
    {
        private readonly IDynamicResourceDataService _dynamicCollectionService;

        public TakeOutDbTrashService(IDynamicResourceDataService dynamicCollectionService)
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
