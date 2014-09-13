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
#if USE_UNITY
            smartFox.ThreadSafeMode = true;
#else
            smartFox.ThreadSafeMode = false;
#endif

            foreach(FieldInfo info in Utility.GetFieldInfo(typeof(SFSEvent), BindingFlags.Public | BindingFlags.Static))
                smartFox.AddEventListener(info.GetValue(null).ToString(), ListenerDelegate);

#if USE_UNITY
            if(PuMain.Setting.IsDebug)
                foreach (LogLevel log in Enum.GetValues(typeof(LogLevel)))
                    smartFox.AddLogListener(log, OnDebugMessage);
#endif

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
                Logger.Log("Connecting...");

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
                Logger.Log("Sending request: {0}{1}", myRequest.Resquest.ToString(), rq.Message.Content.GetDump());
            }
        }

        public void Close()
        {
            smartFox.KillConnection();
        }

        void ListenerDelegate(BaseEvent evt)
        {
            Logger.Log("SFServer: {0}: {1}", evt.Type, JsonUtil.Serialize(evt.Params));
            ISocketResponse response = new SFSocketResponse(evt);

            if(evt.Type == SFSEvent.ROOM_JOIN)
                RoomHandler.Instance.SetCurrentRoom(response);

            if (onResponse != null)
                onResponse(evt.Type, response);

            //if (evt.Type == SFSEvent.LOGIN)
            //{
            //    SFSUser user = (SFSUser)smartFox.MySelf;
            //    Logger.Log(user.Name);

            //    Logger.Log("Count Variables: " + user.GetVariables().Count);
            //    foreach (Sfs2X.Entities.Variables.UserVariable u in user.GetVariables())
            //        Logger.Log(u.Name + " - " + u.GetSFSObjectValue().GetDump());
            //}
        }

        private void OnDebugMessage(BaseEvent evt)
        {
            Logger.Log("{0}: {1}", evt.Type, evt.Params["message"]);
        }
    }
}
