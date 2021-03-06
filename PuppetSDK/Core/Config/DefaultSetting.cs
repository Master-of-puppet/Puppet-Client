﻿using Puppet.Core;
using Puppet.Core.Manager;
using Puppet.Core.Model;
using Puppet.Core.Modules.Buddy;
using Puppet.Core.Modules.Ping;
using Puppet.Core.Network.Socket;
using Puppet.Utils;
using Puppet.Utils.Loggers;
using Puppet.Utils.Storage;
using Puppet.Utils.Threading;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Puppet
{
    public class DefaultSetting : IPuSettings
    {
        public static string domain;
        public static string socketServer;
        public static int socketPort;

        protected IServerMode server, serverWebHttp, serverBundle;
        EPlatform _platform;
        ServerEnvironment _env;
        string zoneName = "FoxPoker";
        Action _actionUpdate;
        bool isDebug = true;

        public void Init()
        {
            _platform = EPlatform.Editor;
            _env = ServerEnvironment.Dev;

            server = new ServerMode(socketServer);
            serverBundle = new WebServerMode(domain);
            serverWebHttp = new WebServerMode(domain);

            InitSocket();

            AfterInit();
        }

        public void ChangeRealtimeServer(string server, int port)
        {
            socketServer = server;
            socketPort = port;
            this.server = new ServerMode(server, port);
        }

        void InitSocket()
        {
            SocketHandler.Instance.Init(new CSmartFox());
            SocketHandler.Instance.AddPlugin(BuddyHandler.Instance.GetAddOn);
            SocketHandler.Instance.AddPlugin(PingHandler.Instance.GetAddOn);
            SocketHandler.Instance.InitSocket();
        }

        public void Reset()
        {
            InitSocket();
        }

        protected virtual void AfterInit()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.1f));
                if (ActionUpdate != null)
                    ActionUpdate();
            }));
            thread.Start();
        }

        public virtual bool UseUnity
        {
            get { return false; }
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

        public virtual string PathCache 
        { 
            get  { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

        public ServerEnvironment Environment { get { return _env; } set { _env = value; } }
        public IServerMode ServerModeHttp { get { return serverWebHttp; } set { serverWebHttp = value; } }
        public IServerMode ServerModeBundle { get { return serverBundle; } set { serverBundle = value; } }
        public IServerMode ServerModeSocket { get { return server; } set { server = value; } }
        public ISocket Socket { get; set; }

        public virtual void ActionPrintLog(ELogType type, object message)
        {
            if (!IsDebug) return;

            string str = message.ToString();
            str = str.Replace(Logger.StartColor(ELogColor.BLUE), "");
            str = str.Replace(Logger.StartColor(ELogColor.CYAN), "");
            str = str.Replace(Logger.StartColor(ELogColor.GREEN), "");
            str = str.Replace(Logger.StartColor(ELogColor.GREY), "");
            str = str.Replace(Logger.StartColor(ELogColor.LIGHTBLUE), "");
            str = str.Replace(Logger.StartColor(ELogColor.LIME), "");
            str = str.Replace(Logger.StartColor(ELogColor.MAGENTA), "");
            str = str.Replace(Logger.StartColor(ELogColor.YELLOW), "");
            str = str.Replace(Logger.EndColor(), "");

            Console.WriteLine(string.Format("{0}: {1}", type.ToString(), str));
            #if USE_DEBUG_CONSOLE
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            for (int i = 3; i < stackTrace.FrameCount; i++)
            {
                System.Diagnostics.StackFrame frame = stackTrace.GetFrame(i);
                Console.WriteLine("- {0}", frame.ToString());
            }
            #endif
            Console.WriteLine();
        }

        public virtual IStorage PlayerPref
        {
            get { return null; }
        }

        public virtual IThread Threading
        {
            get { return SimpleThread.Instance; }
        }

        public virtual IMainMono MainMono
        {
            get { return SimpleMono.Instance; }
        }

        public virtual ENetworkDataType NetworkDataType
        {
            get { return ENetworkDataType.Cable; }
        }

        public virtual string UniqueDeviceIdentification
        {
            get { return Guid.NewGuid().ToString(); }
        }

        public virtual DataClientDetails ClientDetails
        {
            get { return new DataClientDetails(); }
        }

        public Action ActionUpdate
        {
            get { return _actionUpdate; }
            set { _actionUpdate = value; }
        }

        public virtual void OnApplicationPause(bool pauseStatus)
        {
            Logger.LogWarning("OnApplicationPause: " + pauseStatus);
            if(pauseStatus)
                SaveCache();
        }

        public virtual void OnApplicationQuit()
        {
            Logger.LogWarning("OnApplicationQuit");

            SaveCache();

            PuMain.Socket.Disconnect();
        }

        public virtual void OnUpdate()
        {
            if (ActionUpdate != null)
                ActionUpdate();
        }

        private void SaveCache()
        {
            PuDLCache.Instance.SaveCache();
            PuSession.Instance.SaveSession();
            Puppet.Utils.CacheHandler.Instance.SaveFile(null);
        }
    }

    class ServerMode : IServerMode
    {
        string domain;
        int port = 9933;
        public ServerMode(string domain)
        {
            if (!string.IsNullOrEmpty(domain))
                this.domain = domain;
            else
                this.domain = "127.0.0.1";
        }

        public ServerMode(string domain, int port)
        {
            this.domain = domain;
            this.port = port;
        }

        public string GetBaseUrl() { return string.Format("https://{0}:{1}", Domain, Port); }

        public int Port { get { return port; } }

        public string Domain { get { return domain; } }

        public string GetPath(string path) { return string.Format("{0}/{1}", GetBaseUrl(), path); }
    }

    class WebServerMode : IServerMode
    {
        string domain;
        public WebServerMode(string domain)
        {
            if (!string.IsNullOrEmpty(domain))
                this.domain = domain;
            else
                this.domain = "127.0.0.1";
        }

        public string GetBaseUrl() { return string.Format("http://{0}:{1}", Domain, Port); }

        public int Port { get { return 80; } }

        public string Domain { get { return domain; } }

        public string GetPath(string path) { return string.Format("{0}{1}", GetBaseUrl(), path); }
    }

    class SimpleMono : BaseSingleton<SimpleMono>, IMainMono
    {
        public void BeginCoroutine(IEnumerator routine)
        {
            Thread t = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        Thread.Sleep(100);
                        if (!routine.MoveNext())
                            break;
                    }
                }
                finally
                {
                    IDisposable disposable = routine as IDisposable;
                    if (disposable != null)
                        disposable.Dispose();
                }
            });
            t.Start();
        }

        public void EndCoroutine(IEnumerator routine)
        {
        }

        protected override void Init()
        {
        }
   
    }
} 
