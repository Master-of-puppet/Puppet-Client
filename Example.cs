using Puppet.Core.Network.Socket;
using Puppet.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;

namespace Puppet
{
    class Example
    {
        #region IMPLEMENT PUPPET SDK SETTING 
        class Setting : IPuSettings
        {
            ServerMode server;
            EPlatform _platform;
            EEngine _engine;
            string _pathCaching;

            public Setting(EPlatform platform, EEngine engine, string pathCaching)
            {
                server = new ServerMode();
                _platform = platform;
                _engine = engine;
                _pathCaching = pathCaching;
            }

            public EPlatform Platform
            {
                get { return _platform; }
            }

            public EEngine Engine
            {
                get { return _engine; }
            }

            public string PathCache
            {
                get  { return _pathCaching; }
            }

            public ServerEnvironment Environment
            {
                get { return ServerEnvironment.Dev; }
            }

            public IServerMode ServerModeHttp
            {
                get { return server; }
            }
            public IServerMode ServerModeBundle
            {
                get { return server; }
            }
            public IServerMode ServerModeSocket
            {
                get { return server; }
            }

            class ServerMode : IServerMode
            {
                public string GetBaseUrl()
                {
                    return string.Format("https://{0}:{1}", Domain, Port);
                }

                public string Port
                {
                    get { return "8888"; }
                }

                public string Domain
                {
                    get { return "localhost"; }
                }

                public string GetPath(string path)
                {
                    return string.Format("{0}/{1}", GetBaseUrl(), path);
                }
            }
        }
        #endregion

        public static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += RegisterAssembly;
            
            //Initialized Setting before use
            string pathCaching = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Caching.save");
            PuMain.Setting = new Setting(EPlatform.Editor, EEngine.Base, pathCaching);
            #region TYPE TEST SCRIPTS IN HERE

            //TestCaching();

            TestSocket();

            #endregion
            //Wait for Enter to close console.
            Console.ReadLine();
        }

        static Assembly RegisterAssembly(object sender, ResolveEventArgs args)
        {
            //return Assembly.LoadFrom(@"D:\PROJECTS\Personal\Unity3D\PuppetClient\lib\UnityEngine.dll");
            return Assembly.LoadFrom(@"D:\PROJECTS\Personal\Unity3D\PuppetClient\lib\SmartFox2X.dll");
        }

        static void TestCaching()
        {
            Logger.Log(PuMain.Setting.PathCache);

            //CacheHandler.Instance.SetObject("setting_object", PuMain.Setting);

            CacheHandler.Instance.SaveFile((bool status) => {
                Logger.Log("Save cache file {0}: {1}", PuMain.Setting.PathCache, status);
            });

            //Logger.Log(CacheHandler.Instance.GetObject("setting_object").ToString());
        }

        static void TestSocket()
        {
            CSmartFox sf = new CSmartFox();
            sf.Start();

            sf.Connect();

            Thread thread = new Thread(new ThreadStart(
            () => 
                {
                    Thread.Sleep(1000);
                    sf.FixedUpdate();
                }
            ));
            thread.Start();
        }
    }
}
