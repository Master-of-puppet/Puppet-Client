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
    public class DefaultSetting : IPuSettings
    {
        public static string domain;
        IServerMode server, serverWebHttp, serverWebService, serverBundle;
        EPlatform _platform;
        ServerEnvironment _env;
        ISocket _socket;
        string zoneName = "FoxPoker";
        Action _actionUpdate;
        bool isDebug = true;

        public void Init()
        {
            _platform = EPlatform.Editor;
            _env = ServerEnvironment.Dev;

            server = new ServerMode(domain);
            serverWebService = new WebServiceServerMode(domain);
            serverBundle = new WebServerMode(domain);
            serverWebHttp = new WebServerMode(domain);
            
            _socket = new CSmartFox(null);

            AfterInit();
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
            get  { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Caching.save"); }
        }

        public ServerEnvironment Environment { get { return _env; } set { _env = value; } }
        public IServerMode ServerModeHttp { get { return serverWebHttp; } set { serverWebHttp = value; } }
        public IServerMode ServerModeService { get { return serverWebService; } set { serverWebService = value; } }
        public IServerMode ServerModeBundle { get { return serverBundle; } set { serverBundle = value; } }
        public IServerMode ServerModeSocket { get { return server; } set { server = value; } }
        public ISocket Socket { get { return _socket;  } set { _socket = value; } }

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
    }

    class ServerMode : IServerMode
    {
        string domain;
        public ServerMode(string domain)
        {
            if (!string.IsNullOrEmpty(domain))
                this.domain = domain;
            else
                this.domain = "127.0.0.1";
        }

        public string GetBaseUrl() { return string.Format("https://{0}:{1}", Domain, Port); }

        public int Port { get { return 9933; } }

        public string Domain { get { return domain; } }

        public string GetPath(string path) { return string.Format("{0}/{1}", GetBaseUrl(), path); }
    }

    class WebServiceServerMode : IServerMode
    {
        string domain;
        public WebServiceServerMode(string domain)
        {
            if (!string.IsNullOrEmpty(domain))
                this.domain = domain;
            else
                this.domain = "127.0.0.1";
        }

        public string GetBaseUrl() { return string.Format("http://{0}:{1}", Domain, Port); }

        public int Port { get { return 1990; } }

        public string Domain { get { return domain; } }

        public string GetPath(string path) { return string.Format("{0}/puppet/{1}", GetBaseUrl(), path); }
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

        public string GetPath(string path) { return string.Format("{0}/api/{1}", GetBaseUrl(), path); }
    }
}
