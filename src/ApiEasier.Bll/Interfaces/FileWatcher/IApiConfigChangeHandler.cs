namespace ApiEasier.Bll.Interfaces.FileWatcher
{
    public interface IApiConfigChangeHandler
    {
        Task OnConfigDeletedAsync(string configName);
    }
}
