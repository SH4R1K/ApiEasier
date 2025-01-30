using ApiEasier.Bll.Dto;

namespace ApiEasier.Bll.Interfaces.ApiConfigure
{
    /// <summary>
    /// Интерфейс для работы с конфигурационными файлами API-сервисов.
    /// </summary>
    public interface IConfigDynamicApiService
    {
        Task<ApiServiceDto> CreateAsync(ApiServiceDto dto);
        Task<ApiServiceDto> UpdateAsync(ApiServiceDto dto);
        Task<ApiServiceDto> DeleteAsync(ApiServiceDto dto);
        Task<ApiServiceDto> GetAsync(ApiServiceDto dto);

    }
}
