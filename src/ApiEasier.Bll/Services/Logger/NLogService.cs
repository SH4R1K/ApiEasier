using ApiEasier.Bll.Interfaces.Logger;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Bll.Services.Logger
{

    public class NLogService : ILoggerService
    {
        private readonly NLog.Logger _logger;
        public NLogService() 
        { 
            _logger = LogManager.GetCurrentClassLogger();
        }
        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        public void LogError(Exception ex)
        {
            _logger.Error(ex);
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        public void LogWarn(string message)
        {
            _logger.Warn(message);
        }
    }
}
