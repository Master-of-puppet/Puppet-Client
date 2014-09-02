using Puppet.Core.Model;
using Puppet.Core.Network.Socket;
using Puppet.Utils;
using Puppet.Utils.Loggers;
using Puppet.Utils.Storage;
using Puppet.Utils.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Puppet
{
    internal class DefaultSetting : IPuSettings
    {
        DataClientDetails _clientDetails;
        IServerMode server, serverWeb, serverBundle;
        EPlatform _platform;
        ServerEnvironment _env;
        ISocket _socket;
        string zoneName = "FoxPoker";
        Action _actionUpdate;
        bool isDebug = true;

        internal DefaultSetting()
        {
#if UNITY_ANDROID
            _platform = EPlatform.Android;
#elif UNITY_IPHONE
            _platform = EPlatform.iOS;
#elif UNITY_WEBPLAYER
            _platform = EPlatform.WebPlayer;
#else
            _platform = EPlatform.Editor;
#endif

            _env = ServerEnvironment.Dev;
        }

        public void Init()
        {
            server = new ServerMode();
            serverWeb = new WebServerMode();
            serverBundle = new WebServerMode();
            
            _socket = new CSmartFox(null);
            _clientDetails = new DataClientDetails();

#if !USE_UNITY
            Thread thread = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.1f));
                if(ActionUpdate != null)
                    ActionUpdate();
            }));
            thread.Start();
#endif
        }

        public bool IsDebug
        {
            get { return isDebug; }
            set { isDebug = value; }
        }

        public string ZoneName
        {
            get { return zoneName; }
            set { zoneName = value; }
        }

        public EPlatform Platform 
        { 
            get { return _platform; } 
            set { _platform = value; } 
        }

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
        public ISocket Socket { get { return _socket;  } set { _socket = value; } }

        public void ActionChangeScene(string fromScene, string toScene)
        {
#if USE_UNITY
            UnityEngine.Application.LoadLevel(toScene);
#endif
        }

        public void ActionPrintLog(ELogType type, object message)
        {
            if (!PuMain.Setting.IsDebug)
                return;

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

        public IStorage PlayerPref
        {
            get { return UnityPlayerPrefab.Instance; }
        }

        public IThread Threading
        {
            get { 
#if USE_UNITY
                return UnityThread.Instance;
#else
                return SimpleThread.Instance;
#endif
            }
        }

        public DataClientDetails ClientDetails
        {
            get { return _clientDetails; }
            set { _clientDetails = value; }
        }

        public Action ActionUpdate
        {
            get { return _actionUpdate; }
            set { _actionUpdate = value; }
        }
    }

    class ServerMode : IServerMode
    {
        public string GetBaseUrl() { return string.Format("https://{0}:{1}", Domain, Port); }

        public int Port { get { return 9933; } }

        public string Domain { get { return
            "127.0.0.1";
        } }

        public string GetPath(string path) { return string.Format("{0}/{1}", GetBaseUrl(), path); }
    }

    class WebServerMode : IServerMode
    {
        public string GetBaseUrl() { return string.Format("http://{0}:{1}", Domain, Port); }

        public int Port { get { return 1990; } }

        public string Domain { get { return 
            "127.0.0.1";
        } }

        public string GetPath(string path) { return string.Format("{0}/puppet/{1}", GetBaseUrl(), path); }
    }
}
