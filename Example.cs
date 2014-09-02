using Puppet.Core.Flow;
using Puppet.Core.Model;
using Puppet.Core.Network.Http;
using Puppet.Core.Network.Socket;
using Puppet.Utils;
using Puppet.Utils.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace Puppet
{
    class Example
    {
        public static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += RegisterAssembly;

            //Required called before using
            PuMain.Instance.Load();
            
            #region TYPE TEST SCRIPTS IN HERE

            TestGetToken();

            #endregion

            //Wait for Enter to close console.
            Console.ReadLine();
        }

        /// <summary>
        /// Registration Assembly for Console Application
        /// </summary>
        static Assembly RegisterAssembly(object sender, ResolveEventArgs args)
        {
            return Assembly.LoadFrom(@"D:\PROJECTS\Personal\Unity3D\PuppetClient\lib\SmartFox2X.dll");
        }

        static void TestCaching()
        {
            Logger.Log(PuMain.Setting.PathCache);

            CacheHandler.Instance.SaveFile((bool status) => {
                Logger.Log("Save cache file {0}: {1}", PuMain.Setting.PathCache, status);
            });
        }

        static void TestGetToken()
        {
            API.Client.APILogin.GetAccessToken("dungnv", "puppet#89", (IHttpResponse response, bool status, string token) =>
            {
                if(string.IsNullOrEmpty(response.Error))
                    Logger.Log("Status:{0} - Token:{1}", status, token);
                
                if(status)
                    API.Client.APILogin.Login(token, null);
            });
        }
    }
}
