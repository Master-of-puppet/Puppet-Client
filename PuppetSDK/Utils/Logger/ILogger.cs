using System;

namespace Puppet.Utils.Loggers
{
    internal interface ILogger
    {
        void Info(object message, params object[] list);

        void Log(string message);

        void Log(ELogColor color, string message);

        void Log(string message, params object[] list);

        void Log(ELogColor color, string message, params object[] list);

        void LogError(string message, params object[] list);

        void LogWarning(string message, params object[] list);

        void LogException(Exception exception);
    }

    public enum ELogType
    {
        Info,
        Error,
        Warning,
        Exception
    }
}
