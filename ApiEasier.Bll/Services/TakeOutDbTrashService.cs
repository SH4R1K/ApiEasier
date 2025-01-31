using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Server.Dto;
using ApiEasier.Server.Models;

namespace ApiEasier.Bll.Services
{
    public class TakeOutDbTrashService : IHostedService
    {
        private readonly IDynamicResource _dynamicCollectionService;

        public TakeOutDbTrashService(IDynamicResource dynamicCollectionService)
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
