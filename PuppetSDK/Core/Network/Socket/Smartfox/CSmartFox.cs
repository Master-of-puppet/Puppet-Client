using System;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Entities.Data;
using Sfs2X.Entities;
using Puppet.Utils;
using Puppet.Core.Model.Factory;
using Puppet.Core.Model;
using System.Threading;
using Puppet.Core.Network.Socket;
using System.Reflection;
using Sfs2X.Util;
using Sfs2X.Logging;
using System.Text;

namespace Puppet.Core.Network.Socket
{
    internal class CSmartFox : SocketAbstract
    {
        //int TEST_BLUEBOX_PORT = 1313;
        SmartFox smartFox;
        ConfigData configData;

        public SmartFox SmartFox { get { return smartFox; } }
        
        public override void InitSocket()
        {
            smartFox = new SmartFox(true);

            foreach (FieldInfo info in Utility.GetFieldInfo(typeof(SFSEvent), BindingFlags.Public | BindingFlags.Static))
                smartFox.AddEventListener(info.GetValue(null).ToString(), ListenerDelegate);

            //smartFox.SetReconnectionSeconds(10);
            smartFox.ThreadSafeMode = PuMain.Setting.UseUnity;

            //if (PuMain.Setting.UseUnity && PuMain.Setting.IsDebug)
                foreach (LogLevel log in Enum.GetValues(typeof(LogLevel)))
                    smartFox.AddLogListener(log, OnDebugMessage);

            //smartFox.UseBlueBox = PuMain.Setting.NetworkDataType == ENetworkDataType.MobileData;
            //smartFox.UseBlueBox = true;

            //PuMain.ClientDispatcher.onNetworkConnectChange += ClientDispatcher_onNetworkConnectChange;

            base.InitSocket();
        }

        //void ClientDispatcher_onNetworkConnectChange(ENetworkDataType fromData, ENetworkDataType toData)
        //{
        //    smartFox.UseBlueBox = toData == ENetworkDataType.MobileData;
        //}

        public override bool IsConnected
        {
            get { return smartFox.IsConnected; }
        }

        public override void Connect()
        {
            PuMain.Setting.Threading.QueueOnMainThread(() =>
            {
                base.Connect();
                if (!IsConnected)
                {
                    configData = new ConfigData();
                    configData.Host = PuMain.Setting.ServerModeSocket.Domain;
                    //configData.Port = PuMain.Setting.NetworkDataType == ENetworkDataType.MobileData ? TEST_BLUEBOX_PORT :
                    //    PuMain.Setting.ServerModeSocket.Port;
                    configData.Port = PuMain.Setting.ServerModeSocket.Port;
                    configData.Zone = PuMain.Setting.ZoneName;

                    smartFox.Connect(configData);
                    Logger.Log(ELogColor.GREEN, "Connecting smartfox... Host: {0} - Port: {1}", configData.Host, configData.Port);

                    //After connect: Start get event in queue
                    PuMain.Setting.ActionUpdate = () =>
                    {
                        smartFox.ProcessEvents();
                    };
                }
            });
        }

        public override void Disconnect()
        {
            base.Disconnect();
            smartFox.Disconnect();
        }

        public override void Reconnect()
        {
            smartFox.Connect();
        }

        public override void Request(ISocketRequest request)
        {
            SFSocketRequest myRequest = (SFSocketRequest)request;
            smartFox.Send(myRequest.Resquest);

            if (PuMain.Setting.IsDebug)
            {
                BaseRequest rq = (BaseRequest)myRequest.Resquest;
                Logger.Log("Sending request:", rq.Message.Content.GetDump(), ELogColor.MAGENTA);
            }
        }

        public override void Close()
        {
            smartFox.KillConnection();
        }

        void ListenerDelegate(BaseEvent evt)
        {
            if (evt.Type == SFSEvent.HANDSHAKE) return;

            if (PuMain.Setting.IsDebug)
            {
                Logger.Log("SFServer: type [" + evt.Type + "]", JsonUtil.Serialize(evt.Params), ELogColor.CYAN);

                foreach (object key in evt.Params.Keys)
                {
                    object obj = evt.Params[key];
                    if (obj is SFSObject)
                    {
                        SFSObject sfsObject = (SFSObject)obj;

                        string command = string.Empty;
                        if (sfsObject.ContainsKey(Fields.MESSAGE))
                        {
                            ISFSObject message = sfsObject.GetSFSObject(Fields.MESSAGE);
                            command = Utility.GetCommandName(message);
                        }
                        Logger.Log("SFServer [" + key + "]" + (string.IsNullOrEmpty(command) ? "" : "[" + command + "]") + ":", sfsObject.GetDump(), ELogColor.CYAN);
                        //Logger.Log("JsonFormat =>\n", new JsonFormatter(Utility.SFSObjectToString(sfsObject)).Format(), ELogColor.CYAN);
                    }
                }
            }

            ISocketResponse response = new SFSocketResponse(evt);

            if(evt.Type == SFSEvent.ROOM_JOIN)
                RoomHandler.Instance.SetCurrentRoom(response);

            DispatchResponse(evt.Type, response);
        }

        private void OnDebugMessage(BaseEvent evt)
        {
            if (PuMain.Setting.IsDebug)
            {
                Logger.Log("SmartFox - {0}: {1}", evt.Type, evt.Params[Fields.MESSAGE]);
            }
        }
    }
}
