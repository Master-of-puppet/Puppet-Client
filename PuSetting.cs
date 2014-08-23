//#define USE_UNITY

using Puppet.Utils.Loggers;
using Puppet.Utils.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Puppet
{
    internal class PuSetting : BaseSingleton<PuSetting>, IPuSettings
    {
        IServerMode server, serverWeb, serverBundle;
        EPlatform _platform;
        ServerEnvironment _env;

        public override void Init()
        {
            server = new ServerMode();
            serverWeb = new WebServerMode();
            serverBundle = new WebServerMode();
            _platform = EPlatform.Editor;
            _env = ServerEnvironment.Dev;
        }

        public EPlatform Platform { get { return _platform; } set { _platform = value; } }

        public string PathCache 
        { 
            get 
            {
                string fileName = "Caching.save";

                string directory = string.Empty;
#if USE_UNITY
                directory = UnityEngine.Application.dataPath;
#else
                directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
#endif
                return Path.Combine(directory, fileName);
            }
        }

        public ServerEnvironment Environment { get { return _env; } set { _env = value; } }
        public IServerMode ServerModeHttp { get { return serverWeb; } set { serverWeb = value; } }
        public IServerMode ServerModeBundle { get { return serverBundle; } set { serverBundle = value; } }
        public IServerMode ServerModeSocket { get { return server; } set { server = value; } }

        public void ActionChangeScene(string fromScene, string toScene)
        {
#if USE_UNITY
            UnityEngine.Application.LoadLevel(toScene);
#endif
        }

        public void ActionPrintLog(ELogType type, object message)
        {
#if USE_UNITY
            switch(type)
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
            StackTrace();
#endif
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


        public Utils.Storage.IStorage PlayerPref
        {
            get 
            { 
                return UnityPlayerPrefab.Instance; 
            }
        }

        public Utils.Threading.IThread Threading
        {
            get { 
#if USE_UNITY
                return UnityThread.Instance;
#else
                return SimpleThread.Instance;
#endif
            }
        }
    }

    class ServerMode : IServerMode
    {
        public string GetBaseUrl() { return string.Format("https://{0}:{1}", Domain, Port); }

        public string Port { get { return "9933"; } }

        public string Domain { get { return "test.esimo.vn"; } }

        public string GetPath(string path) { return string.Format("{0}/{1}", GetBaseUrl(), path); }
    }

    class WebServerMode : IServerMode
    {
        public string GetBaseUrl() { return string.Format("http://{0}:{1}", Domain, Port); }

        public string Port { get { return "1990"; } }

        public string Domain { get { return "test.esimo.vn"; } }

        public string GetPath(string path) { return string.Format("{0}/puppet/{1}", GetBaseUrl(), path); }
    }
}
