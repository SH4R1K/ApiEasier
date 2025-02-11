using ApiEasier.Server.Dto;

namespace ApiEasier.Server.Interfaces
{
    public interface IApiListHub
    {
        public Task RecieveMessage(List<ShortApiServiceDto> apiServices);
        public Task AddService(ShortApiServiceDto apiService);
        public Task RemoveService(string apiService);
        public Task UpdateService(string oldName, ShortApiServiceDto apiService);
        public Task UpdateStatusService(string name, bool isActive);
    }
}
