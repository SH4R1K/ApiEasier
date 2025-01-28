namespace ApiEasier.Domain.Interfaces
{
    public interface IJsonRepository
    {
        Task<T?> ReadJsonAsync<T>(string fileName);
        Task WriteJsonAsync<T>(string fileName, T data);
    }
}
