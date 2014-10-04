using System;
using Puppet.Utils.Loggers;

namespace Puppet
{
    public class Logger : BaseSingleton<Logger>
    {
        ILogger _logger;

        protected override void Init()
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

        public static void Log(ELogColor color, string message)
        {
            Instance._logger.Log(color, message);
        }

        public static void Log(ELogColor color, string message, params object[] list)
        {
            Instance._logger.Log(color, message, list);
        }

        public static void Log(string message1, string message2, ELogColor color)
        {
            Instance._logger.Log("{0}{1}{2} - {3}", Logger.StartColor(color), message1, Logger.EndColor(), message2);
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

        public static string StartColor(ELogColor color)
        {
            return "<color=" + color.ToString().ToLower() + ">";
        }
        public static string EndColor()
        {
            return "</color>";
        }
    }

    public enum ELogColor
    {
        NONE,
        YELLOW,
        GREEN,
        BLUE,
        CYAN,
        LIME,
        LIGHTBLUE,
        GREY,
        MAGENTA
    }
}
