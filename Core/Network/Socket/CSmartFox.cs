using System;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;

namespace Puppet.Core.Network.Socket
{
    public class CSmartFox 
    {
        SmartFox smartFox;
        public void Start()
        {
            smartFox = new SmartFox(true);
            

            smartFox.AddLogListener(Sfs2X.Logging.LogLevel.ERROR, ListenerDelegateLog);
            smartFox.AddLogListener(Sfs2X.Logging.LogLevel.INFO, ListenerDelegateLog);
            smartFox.AddLogListener(Sfs2X.Logging.LogLevel.WARN, ListenerDelegateLog);

            smartFox.AddEventListener(SFSEvent.CONNECTION, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.CONNECTION_ATTEMPT_HTTP, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.CONNECTION_LOST, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.CONNECTION_RESUME, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.CONNECTION_RETRY, ListenerDelegate);

            smartFox.AddEventListener(SFSEvent.LOGIN, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.LOGIN_ERROR, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.LOGOUT, ListenerDelegate);


            smartFox.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        }

        public void Connect()
        {
            if (!smartFox.IsConnected)
            {
                smartFox.Connect("test.esimo.vn", 9933);
                Logger.Log("Connecting...");
            }  
        }

        void OnConnection(BaseEvent evt)
        {
            if (evt.Params.Contains("success"))
            {
                bool success = (bool)evt.Params["success"];
                if(success)
                {
                    Logger.Log("LoginRequest Sending...");
                    smartFox.Send(new LoginRequest("dungnv", "puppet#89", "BasicExamples"));
                }
            }
        }

        void ListenerDelegate(BaseEvent evt)
        {
            Logger.Log("SFServer: {0}: {1}", evt.Type, MiniJSON.Json.Serialize(evt.Params));
        }

        public void ListenerDelegateLog(BaseEvent evt)
        {
            Logger.Log("SFLog: {0}: {1}", evt.Type, MiniJSON.Json.Serialize(evt.Params));
        }

        public void FixedUpdate()
        {
            smartFox.ProcessEvents();
        }
    }
}
