using ApiEasier.Logger.Interfaces;
using Microsoft.AspNetCore.Http;
using NLog;

namespace ApiEasier.Logger.Services
{
    public class NLogService : ILoggerService
    {
        private readonly NLog.Logger _logger;
        public NLogService()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }
        public void LogHttp(HttpContext context, string requestBody, string responseBody)
        {
            var logEventInfo = new LogEventInfo(NLog.LogLevel.Info, _logger.Name, $"{context.Request.Path} | {context.Request.Method} | {context.Response.StatusCode}")
            {
                Properties =
            {
                ["Timestamp"] = DateTime.UtcNow,
                ["Protocol"] = context.Request.Protocol,
                ["Method"] = context.Request.Method,
                ["Scheme"] = context.Request.Scheme,
                ["Path"] = context.Request.Path,
                ["QueryString"] = context.Request.QueryString.ToString(),
                ["Accept"] = context.Request.Headers["Accept"].ToString(),
                ["Host"] = context.Request.Headers["Host"].ToString(),
                ["User-Agent"] = context.Request.Headers["User-Agent"].ToString(),
                ["Accept-Encoding"] = context.Request.Headers["Accept-Encoding"].ToString(),
                ["Accept-Language"] = context.Request.Headers["Accept-Language"].ToString(),
                ["Referer"] = context.Request.Headers["Referer"].ToString(),
                ["RequestBody"] = requestBody,
                ["ResponseBody"] = responseBody,
                ["ResponseCode"] = context.Response.StatusCode,
            }
            };

            _logger.Log(logEventInfo);
        }

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        public void LogError(Exception ex, string message)
        {
            _logger.Error(ex, message);
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        public void LogWarn(string message)
        {
            _logger.Warn(message);
        }
        public void LogFatal(Exception ex, string message)
        {
            _logger.Fatal(ex, message);
        }
    }
}
