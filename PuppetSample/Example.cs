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
            PuMain.Instance.Load(null);

            PuMain.Dispatcher.onChangeScene += Instance_onChangeScene;
            PuMain.Dispatcher.onDailyGift += Dispatcher_onDailyGift;

            AtLoginScreen();
        }

        static void Dispatcher_onDailyGift(DataDailyGift obj)
        {
            API.Client.APIGeneric.GetDailyGift();
        }

        static void Instance_onChangeScene(EScene fromScene, EScene toScene)
        {
            if (toScene == EScene.World_Game)
                AtWorldScene();
            else if (toScene == EScene.Pocker_Lobby)
                AtLobbyScene();
            else if (toScene == EScene.Pocker_Gameplay)
                AtGameplay();
            else if (toScene == EScene.LoginScreen)
                AtLoginScreen();
        }

        /// <summary>
        /// Registration Assembly for Console Application
        /// </summary>
        static Assembly RegisterAssembly(object sender, ResolveEventArgs args)
        {
            return Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + @"\SmartFox2X.dll");
        }

        static void InputSetting()
        {
            Console.WriteLine("1. foxpokers.com");
            Console.WriteLine("2. puppet.esimo.vn");
            Console.WriteLine("3. localhost");
            Console.WriteLine("Or enter you server address");
            Console.Write("Please enter server: ");
            string enter = Console.ReadLine();
            if (enter.EndsWith("1"))
                enter = "foxpokers.com";
            else if (enter.EndsWith("2"))
                enter = "puppet.esimo.vn";
            else if (enter.EndsWith("3"))
                enter = "127.0.0.1";
            DefaultSetting.domain = DefaultSetting.socketServer = enter;
            //DefaultSetting.soketServer = "192.168.10.143";

            Console.Write("Enter your Username: ");
            userName = Console.ReadLine();
            Console.Write("Enter your Password: ");
            password = Console.ReadLine();
            userName = string.IsNullOrEmpty(userName) ? "dungnv" : userName;
            password = string.IsNullOrEmpty(password) ? "puppet#89" : password;
            Console.WriteLine(string.Format("Server: {0} - Username: {1}", enter, userName));
        }

        static void AtLoginScreen()
        {
            //API.Client.APILogin.GetAccessToken(userName, password, (bool status, string token, IHttpResponse response) =>
            //{
            //    if (status)
            //        API.Client.APILogin.Login(token, null);
            //});
            Logger.Log("Login.....");
            API.Client.APILogin.LoginTrial(null);

            Puppet.Poker.PokerMain.Instance.StartListen();
        }

        static void AtWorldScene()
        {
            UserInfo uerInfo = API.Client.APIUser.GetUserInformation();
            Console.WriteLine("Username: " + uerInfo.info.userName);
            Console.WriteLine("Chip: " + uerInfo.assets.GetAsset(EAssets.Chip).value);

            API.Client.APIWorldGame.GetListGame((bool getStatus, string getMessage, List<DataGame> listGame) =>
            {
                if (getStatus && listGame.Count > 0)
                {
                    int i = 0;
                    for (; i < listGame.Count; i++)
                        Console.WriteLine(i + ". To choose game " + listGame[i].roomName);
                    Console.WriteLine(i + ". To Logout");
                    int choose = GetEnterInteger(0, listGame.Count);
                    if (choose == i)
                    {
                        //API.Client.APIGeneric.BackScene(null);
                        API.Client.APIGeneric.LoginOut(null);
                    }
                    else
                        API.Client.APIWorldGame.JoinRoom(listGame[choose], null);
                }
                else
                    Logger.Log("Hiện chưa có trò chơi nào.");
            });
        }

        static void AtLobbyScene()
        {
            //Puppet.Poker.PokerMain.Instance.StartListen();
            API.Client.APILobby.GetGroupsLobby((bool getGStatus, string getGMessage, List<DataChannel> data) =>
            {
                int i = 0;
                for (; i < data.Count; i++)
                    Console.WriteLine(i + ". To choose channel " + data[i].displayName);
                Console.WriteLine(i + ". Back");
                int choose = GetEnterInteger(0, data.Count);
                if (choose == i)
                    API.Client.APIGeneric.BackScene(null);
                else
                {
                    API.Client.APILobby.SetSelectChannel(data[choose], (bool gStatus, string gMessage, List<DataLobby> listChildren) =>
                    {
                        Console.WriteLine("ListChildren Count: " + listChildren.Count);
                        Console.WriteLine("0. Create new game");
                        int j = 1;
                        if (listChildren.Count > 0)
                        {
                            for (; j <= listChildren.Count; j++)
                                Console.WriteLine(j + ". To choose game " + listChildren[j-1].displayName);
                        }
                        Console.WriteLine(j + ". Back");
                        choose = GetEnterInteger(0, j);

                        if (choose == 0)
                            API.Client.APILobby.CreateLobby(1000, 9, null);
                        else if (choose == j)
                            API.Client.APIGeneric.BackScene(null);
                        else
                            API.Client.APILobby.JoinLobby(listChildren[choose - 1], null);
                    });
                }
            });
        }

        static void AtGameplay()
        {
            Console.WriteLine("0. Back");
            Console.WriteLine("1. Sit Down Slot index 1");
            int choose = GetEnterInteger(0, 1);
            if (choose == 0)
                API.Client.APIGeneric.BackScene(null);
            else if (choose == 1)
                API.Client.APIPokerGame.SitDown(1, 500, true);
        }
    }
}
