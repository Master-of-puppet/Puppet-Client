using Puppet.Utils;
using System;
using System.Collections.Generic;

namespace Puppet
{
    class Example
    {
        #region IMPLEMENT PUPPET SDK SETTING 
        class Setting : IPuSettings
        {
            ServerMode server;
            public Setting()
            {
                server = new ServerMode();
            }

            public EPlatform Platform
            {
                get { return EPlatform.Editor; }
            }

            public EEngine Engine
            {
                get { return EEngine.Base; }
            }

            public string PathCache
            {
                get 
                {
                    string directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    return System.IO.Path.Combine(directory, "Caching.save"); 
                }
            }

            public ServerEnvironment Environment
            {
                get { return ServerEnvironment.Dev; }
            }

            public IServerMode ServerModeWeb
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
            //Initialized Setting before use
            PuMain.Setting = new Setting();
            #region TYPE TEST SCRIPTS IN HERE

            TestCaching();

            #endregion
            //Wait for Enter to close console.
            Console.ReadLine();
        }

        static void TestCaching()
        {
            Logger.Log(PuMain.Setting.PathCache);

            CacheHandler.Instance.SetString("string_key", "string_value");

            CacheHandler.Instance.SaveFile((bool status) => {
                Logger.Log("Save cache file {0}: {1}", PuMain.Setting.PathCache, status);
            });
        }
    }
}
