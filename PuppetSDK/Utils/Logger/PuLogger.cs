using System;

namespace Puppet.Utils.Loggers
{
    class PuLogger : ILogger
    {
        public void Info(object message, params object[] list)
        {
            Log(message.ToString(), list);
        }

        public void Log(string message)
        {
            PuMain.Setting.ActionPrintLog(ELogType.Info, message);
        }

        public void Log(string message, params object[] list)
        {
            PuMain.Setting.ActionPrintLog(ELogType.Info, string.Format(message, list));
        }

        public void LogError(string message, params object[] list)
        {
            PuMain.Setting.ActionPrintLog(ELogType.Error, string.Format(message, list));
        }

        public void LogWarning(string message, params object[] list)
        {
            PuMain.Setting.ActionPrintLog(ELogType.Warning, string.Format(message, list));
        }

        public void LogException(Exception exception)
        {
            PuMain.Setting.ActionPrintLog(ELogType.Exception, exception);
        }
    }
}
