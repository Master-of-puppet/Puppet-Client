﻿using Puppet.Core.Flow;
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
        static CSmartFox sf;
        public static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += RegisterAssembly;
            
            //Initialized Setting before use
            #region TYPE TEST SCRIPTS IN HERE

            //TestCaching();

            //TestSocket();

            //TestSceneFlow();

            TestGetToken();

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

            CacheHandler.Instance.SaveFile((bool status) => {
                Logger.Log("Save cache file {0}: {1}", PuMain.Setting.PathCache, status);
            });
        }

        static void StartFixedUpdate()
        {
            Thread thread = new Thread(new ThreadStart(
            () => 
                {
                    Thread.Sleep(1000);
                    if(sf != null)
                        sf.FixedUpdate();
                }
            ));
            thread.Start();
        }

        static void TestGetToken()
        {
            API.APIAuthentication.GetAccessToken("dungnv", "puppet#89", (IHttpResponse response, bool status, string token) =>
            {
                Logger.Log("Status:{0} - Token:{1}", status, token);

                if(status)
                {
                    sf = new CSmartFox(token);
                    StartFixedUpdate();
                    sf.Connect();
                }
            });
        }
    }
}
