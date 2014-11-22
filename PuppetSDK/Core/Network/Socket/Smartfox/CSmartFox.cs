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
    internal class CSmartFox : ISocket
    {
        public event Action<string, ISocketResponse> onResponse;
        SmartFox smartFox;
        ConfigData configData;
        
        public CSmartFox(Action<string, ISocketResponse> onEventResponse)
        {
            smartFox = new SmartFox(true);

            //smartFox.SetReconnectionSeconds(10);
            smartFox.ThreadSafeMode = PuMain.Setting.UseUnity;
            //smartFox.EnableLagMonitor(true, 3, 10);

            foreach(FieldInfo info in Utility.GetFieldInfo(typeof(SFSEvent), BindingFlags.Public | BindingFlags.Static))
                smartFox.AddEventListener(info.GetValue(null).ToString(), ListenerDelegate);

            //if (PuMain.Setting.UseUnity && PuMain.Setting.IsDebug)
            //    foreach (LogLevel log in Enum.GetValues(typeof(LogLevel)))
            //        smartFox.AddLogListener(log, OnDebugMessage);

            if (onEventResponse != null)
                AddListener(onEventResponse);
        }

        public void AddListener(Action<string, ISocketResponse> onEventResponse)
        {
            this.onResponse += onEventResponse;
        }

        public void RemoveListener(Action<string, ISocketResponse> onEventResponse)
        {
            this.onResponse -= onEventResponse;
        }

        public bool IsConnected
        {
            get { return smartFox.IsConnected; }
        }

        public void Connect()
        {
            if (!IsConnected)
            {
                configData = new ConfigData();
                configData.Host = PuMain.Setting.ServerModeSocket.Domain;
                configData.Port = PuMain.Setting.ServerModeSocket.Port;
                configData.Zone = PuMain.Setting.ZoneName;

                smartFox.Connect(configData);
                Logger.Log(ELogColor.GREEN, "Connecting...");

                //After connect: Start get event in queue
                PuMain.Setting.ActionUpdate = ProcessEvents;
            }
        }

        public void Reconnect()
        {
            smartFox.Connect();
        }

        public void Disconnect()
        {
            smartFox.Disconnect();
        }

        public void ProcessEvents()
        {
            smartFox.ProcessEvents();
        }

        public void Request(ISocketRequest request)
        {
            SFSocketRequest myRequest = (SFSocketRequest)request;
            smartFox.Send(myRequest.Resquest);

            if (PuMain.Setting.IsDebug)
            {
                BaseRequest rq = (BaseRequest)myRequest.Resquest;
                Logger.Log("Sending request:", rq.Message.Content.GetDump(), ELogColor.MAGENTA);
            }
        }

        public void Close()
        {
            smartFox.KillConnection();
        }

        void ListenerDelegate(BaseEvent evt)
        {
            if (evt.Type == SFSEvent.HANDSHAKE) return;

            Logger.Log("SFServer: type [" +  evt.Type + "]", JsonUtil.Serialize(evt.Params), ELogColor.CYAN);

            foreach(object key in evt.Params.Keys)
            {
                object obj = evt.Params[key];
                if(obj is SFSObject)
                {
                    SFSObject sfsObject = (SFSObject)obj;
                    Logger.Log("SFServer [" + key + "]:", sfsObject.GetDump(), ELogColor.CYAN);
                    Logger.Log("JsonFormat =>\n", new JsonFormatter(Utility.SFSObjectToString(sfsObject)).Format(), ELogColor.CYAN);
                }
            }

            ISocketResponse response = new SFSocketResponse(evt);

            if(evt.Type == SFSEvent.ROOM_JOIN)
                RoomHandler.Instance.SetCurrentRoom(response);

            if (onResponse != null)
                onResponse(evt.Type, response);
        }

        private void OnDebugMessage(BaseEvent evt)
        {
            Logger.Log("{0}: {1}", evt.Type, evt.Params["message"]);
        }
    }
}
