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

        public void Log(ELogColor color, string message)
        {
            if (PuMain.Setting.UseUnity)
                PuMain.Setting.ActionPrintLog(ELogType.Info, string.Format("<color={0}>{1}</color>", color.ToString().ToLower(), message));
            else
                Log(message);
        }

        public void Log(ELogColor color, string message, params object[] list)
        {
            if (PuMain.Setting.UseUnity)
                PuMain.Setting.ActionPrintLog(ELogType.Info, "<color=" + color.ToString().ToLower() + ">" + string.Format(message, list) + "</color>");
            else
                Log(message, list);
        }

        public void LogError(string message)
        {
            PuMain.Setting.ActionPrintLog(ELogType.Error, message);
        }

        public void LogError(string message, params object[] list)
        {
            PuMain.Setting.ActionPrintLog(ELogType.Error, string.Format(message, list));
        }

        public void LogWarning(string message)
        {
            PuMain.Setting.ActionPrintLog(ELogType.Warning, message);
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
