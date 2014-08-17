using System;

namespace Puppet.Utils.Loggers
{
    class CSharpLogger : ILogger
    {
        public void Info(object message, params object[] list)
        {
            Log(message.ToString(), list);
        }

        public void Log(string message)
        {
            Console.WriteLine("Log: " + message);
            StackTrace();
        }

        public void Log(string message, params object[] list)
        {
            Console.WriteLine("Log: " + string.Format(message, list));
            StackTrace();
        }

        public void LogError(string message, params object[] list)
        {
            Console.WriteLine("Error: " + string.Format(message, list));
            StackTrace();
        }

        public void LogWarning(string message, params object[] list)
        {
            Console.WriteLine("Warning: " + string.Format(message, list));
            StackTrace();
        }

        public void LogException(Exception exception)
        {
            Console.WriteLine("Exception: " + exception.Message);
            StackTrace();
        }

        void StackTrace()
        {
            //System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            //for (int i = 3; i < stackTrace.FrameCount;i++)
            //{
            //    System.Diagnostics.StackFrame frame = stackTrace.GetFrame(i);
            //    Console.WriteLine("- {0}", frame.ToString());
            //}
            Console.WriteLine();
        }
    }
}
