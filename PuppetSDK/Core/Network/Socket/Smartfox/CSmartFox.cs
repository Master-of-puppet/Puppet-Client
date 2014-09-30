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

namespace Puppet.Core.Network.Socket
{
    internal class CSmartFox : ISocket
    {
        public event Action<string, ISocketResponse> onResponse;
        SmartFox smartFox;
        
        public CSmartFox(Action<string, ISocketResponse> onEventResponse)
        {
            smartFox = new SmartFox(true);

            smartFox.SetReconnectionSeconds(15);
            smartFox.ThreadSafeMode = PuMain.Setting.UseUnity;

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
                ConfigData cfg = new ConfigData();
                cfg.Host = PuMain.Setting.ServerModeSocket.Domain;
                cfg.Port = PuMain.Setting.ServerModeSocket.Port;
                cfg.Zone = PuMain.Setting.ZoneName;

                smartFox.Connect(cfg);
                Logger.Log(ELogColor.GREEN, "Connecting...");

                //After connect: Start get event in queue
                PuMain.Setting.ActionUpdate = ProcessEvents;
            }
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
                Logger.Log("{0}Sending request: {1}{2}{3}", 
                    Logger.StartColor(ELogColor.MAGENTA), 
                    myRequest.Resquest.ToString(), 
                    Logger.EndColor(), 
                    rq.Message.Content.GetDump());
            }
        }

        public void Close()
        {
            smartFox.KillConnection();
        }

        void ListenerDelegate(BaseEvent evt)
        {
            Logger.Log("{0}SFServer: {1}:{2} {3}",
                Logger.StartColor(ELogColor.CYAN), 
                evt.Type,
                Logger.EndColor(), 
                JsonUtil.Serialize(evt.Params));
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
