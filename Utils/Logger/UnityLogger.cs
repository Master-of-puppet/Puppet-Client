using System;
using UnityEngine;

namespace Puppet.Utils.Loggers
{
    class UnityLogger : ILogger
    {
        public void Info(object message, params object[] list)
        {
            Log(message.ToString(), list);
        }

        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void Log(string message, params object[] list)
        {
            Debug.Log(string.Format(message, list));
        }

        public void LogError(string message, params object[] list)
        {
            Debug.LogError(string.Format(message, list));
        }

        public void LogWarning(string message, params object[] list)
        {
            Debug.LogWarning(string.Format(message, list));
        }

        public void LogException(Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}
