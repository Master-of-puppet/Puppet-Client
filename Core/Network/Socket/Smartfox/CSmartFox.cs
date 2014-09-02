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
            Logger.Log("Sending request: {0}", myRequest.Resquest.ToString());
        }

        public void Close()
        {
            smartFox.KillConnection();
        }

        void ListenerDelegate(BaseEvent evt)
        {
            Logger.Log("SFServer: {0}: {1}", evt.Type, MiniJSON.Json.Serialize(evt.Params));
            if (onResponse != null)
                onResponse(evt.Type, new SFSocketResponse(evt));
        }
        
        private void OnDebugMessage(BaseEvent evt)
        {
            Logger.Log("{0}: {1}", evt.Type, evt.Params["message"]);
        }
    }
}
