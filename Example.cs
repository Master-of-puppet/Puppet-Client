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
        static string userName;
        static string password;

        static int GetEnterInteger(int min, int max)
        {
            int id = -1;
            do
            {
                Console.Write("Chooise: ");
                string enter = Console.ReadLine();
                int.TryParse(enter, out id);
            }
            while (id < min || id > max);
            return id;
        }

        public static void Main()
        {
            InputSetting();

            AppDomain.CurrentDomain.AssemblyResolve += RegisterAssembly;

            //Required called before using
            PuMain.Instance.Load();

            GetAccessToken();
        }

        /// <summary>
        /// Registration Assembly for Console Application
        /// </summary>
        static Assembly RegisterAssembly(object sender, ResolveEventArgs args)
        {
            return Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + @"\lib\SmartFox2X.dll");
        }

        static void InputSetting()
        {
            Console.WriteLine("1. test.esimo.vn");
            Console.WriteLine("2. localhost");
            Console.WriteLine("Or enter you server address");
            Console.Write("Please enter server: ");
            string enter = Console.ReadLine();
            if (enter.EndsWith("1"))
                enter = "test.esimo.vn";
            else if (enter.EndsWith("2"))
                enter = "127.0.0.1";
            DefaultSetting.domain = enter;
            Console.Write("Enter your Username: ");
            userName = Console.ReadLine();
            Console.Write("Enter your Password: ");
            password = Console.ReadLine();
            userName = string.IsNullOrEmpty(userName) ? "dungnv" : userName;
            password = string.IsNullOrEmpty(password) ? "puppet#89" : password;
            Console.WriteLine(string.Format("Server: {0} - Username: {1}", enter, userName));
        }

        static void TestCaching()
        {
            Logger.Log(PuMain.Setting.PathCache);

            CacheHandler.Instance.SaveFile((bool status) => {
                Logger.Log("Save cache file {0}: {1}", PuMain.Setting.PathCache, status);
            });
        }

        static void GetAccessToken()
        {
            API.Client.APILogin.GetAccessToken(userName, password, (bool status, string token, IHttpResponse response) =>
            {
                if(status)
                    API.Client.APILogin.Login(token, LoginCallback);
            });
        }

        static void LoginCallback(bool status, string message)
        {
            if(status)
            {
                API.Client.APIWorldGame.GetListGame((bool getStatus, string getMessage, List<DataGame> listGame) =>
                {
                    if (getStatus && listGame.Count > 0)
                    {
                        for (int i = 0; i < listGame.Count; i++)
                            Console.WriteLine(i + ". To choose game " + listGame[i].roomName);
                        int choose = GetEnterInteger(0, listGame.Count - 1);

                        API.Client.APIWorldGame.JoinRoom(listGame[choose], JoinGameCallback);
                    }
                    else
                        Logger.Log("Hiện chưa có trò chơi nào.");
                });
            }
        }

        static void JoinGameCallback(bool status, string message)
        {
            if (status)
            {
                API.Client.APILobby.GetGroupsLobby((bool getGStatus, string getGMessage, List<DataChannel> data) =>
                {
                    for (int i = 0; i < data.Count; i++)
                        Console.WriteLine(i + ". To choose channel " + data[i].name);
                    int choose = GetEnterInteger(0, data.Count - 1);

                    API.Client.APILobby.SetSelectChannel(data[choose], (bool gStatus, string gMessage, List<DataLobby> listChildren) =>
                    {
                        Console.WriteLine("0. Create new game");
                        Console.WriteLine("1. Back");
                        choose = GetEnterInteger(0, 1);

                        switch (choose)
                        {
                            case 0:
                                API.Client.APILobby.CreateLobby(null);
                                break;
                            case 1:
                                API.Client.APIGeneric.BackScene(null);
                                break;
                        }
                    });
                });
            }
        }
    }
}
