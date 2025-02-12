namespace ApiEasier.Bll.Interfaces.Logger
{
    public interface ILoggerService
    {
        public void LogInfo(string message);
        public void LogWarn(string message);
        public void LogError(Exception ex);
        public void LogDebug(string message);
    }
}
