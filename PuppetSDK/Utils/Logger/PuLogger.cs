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
            ActionPrintLog(ELogType.Info, message);
        }

        public void Log(string message, params object[] list)
        {
            ActionPrintLog(ELogType.Info, string.Format(message, list));
        }

        public void LogError(string message, params object[] list)
        {
            ActionPrintLog(ELogType.Error, string.Format(message, list));
        }

        public void LogWarning(string message, params object[] list)
        {
            ActionPrintLog(ELogType.Warning, string.Format(message, list));
        }

        public void LogException(Exception exception)
        {
            ActionPrintLog(ELogType.Exception, exception);
        }

        public void ActionPrintLog(ELogType type, object message)
        {
#if USE_UNITY
            switch (type)
            {
                case ELogType.Info:
                    UnityEngine.Debug.Log(message);
                    break;
                case ELogType.Warning:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                case ELogType.Error:
                    UnityEngine.Debug.LogError(message);
                    break;
                case ELogType.Exception:
                    UnityEngine.Debug.LogException((Exception)message);
                    break;
            }
#else
            Console.WriteLine(string.Format("{0}: {1}", type.ToString(), message.ToString()));
#if USE_DEBUG_CONSOLE
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            for (int i = 3; i < stackTrace.FrameCount; i++)
            {
                System.Diagnostics.StackFrame frame = stackTrace.GetFrame(i);
                Console.WriteLine("- {0}", frame.ToString());
            }
#endif
            Console.WriteLine();
#endif
        }
    }
}
