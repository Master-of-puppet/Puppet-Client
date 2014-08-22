using System;
using Puppet.Utils.Loggers;

namespace Puppet
{
    public class Logger : BaseSingleton<Logger>
    {
        ILogger _logger;

        public override void Init()
        {
            _logger = new PuLogger();
        }

        public static void Info(object message, params object[] list)
        {
            Instance._logger.Info(message, list);
        }

        public static void Log(string message)
        {
            Instance._logger.Log(message);
        }

        public static void Log(string message, params object[] list)
        {
            Instance._logger.Log(message, list);
        }

        public static void LogError(string message, params object[] list)
        {
            Instance._logger.LogError(message, list);
        }

        public static void LogWarning(string message, params object[] list)
        {
            Instance._logger.LogWarning(message, list);
        }

        public static void LogException(System.Exception exception)
        {
            Instance._logger.LogException(exception);
        }
    }
}
