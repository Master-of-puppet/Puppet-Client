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
            API.Client.APILogin.GetAccessToken("dungnv", "puppet#89", (bool status, string token, IHttpResponse response) =>
            {
                if(status)
                    API.Client.APILogin.Login(token, LoginCallback);
            });
        }

        static void LoginCallback(bool status, string message)
        {
            if(status)
            {
                API.Client.APIGame.GetListGame((bool getStatus, string getMessage, List<DataGame> listGame) =>
                {
                    if (getStatus && listGame.Count > 0)
                        API.Client.APIGame.JoinRoom(listGame[0], JoinGameCallback);
                    else
                        Logger.Log("Hiện chưa có trò chơi nào.");
                });
            }
        }

        static void JoinGameCallback(bool status, string message)
        {
            if(status)
            {
                API.Client.APILobby.GetAllLobby((bool getStatus, string getMessage, List<DataLobby> listLobby) =>
                {
                    if (listLobby.Count > 0)
                    {
                        API.Client.APILobby.SetSelectGroup(listLobby[0].groupName, (bool gStatus, string gMessage, List<DataLobby> listChildren) =>
                        {
                            foreach (DataLobby Lobby in listChildren)
                            {
                                Logger.Log("Lobby: " + Lobby.ToSFSObject().GetDump());
                            }
                        });
                    }

                    API.Client.APILobby.GetAllGroupName((bool getGStatus, string getGMessage, object data) =>
                    {
                        List<string> groups = (List<string>)data;
                        foreach (string groupName in groups)
                        {
                            Logger.Log("GroupName: " + groupName);
                        }
                    });

                });
            }
        }
    }
}
