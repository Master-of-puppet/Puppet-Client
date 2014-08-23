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

namespace Puppet.Core.Network.Socket
{
    public class CSmartFox 
    {
        SmartFox smartFox;
        string accessToken = string.Empty;
        public CSmartFox(string token)
        {
            smartFox = new SmartFox(true);
            accessToken = token;

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

            smartFox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, ListenerDelegate);

            smartFox.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        }

        public void Connect()
        {
            if (!smartFox.IsConnected)
            {
                smartFox.Connect(PuMain.Setting.ServerModeSocket.Domain, int.Parse(PuMain.Setting.ServerModeSocket.Port));
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
                    ISFSObject obj = new SFSObject();
                    obj.PutUtfString("token", accessToken);
                    smartFox.Send(new LoginRequest(string.Empty, string.Empty, "FoxPoker", obj));

                    ExtensionRequest request = new ExtensionRequest("test", new SFSObject());
                    smartFox.Send(request);
                }
            }
        }

        void ListenerDelegate(BaseEvent evt)
        {
            Logger.Log("SFServer: {0}: {1}", evt.Type, MiniJSON.Json.Serialize(evt.Params));

            if(evt.Type == "extensionResponse")
            {
                SFSObject obj = (SFSObject)evt.Params["params"];
                DataTest test = SFSDataModelFactory.CreateDataModel<DataTest>(obj);
                Logger.Log(test.ToString());
            }


            if (evt.Type == "login")
            {
                SFSObject obj = (SFSObject)evt.Params["data"];
                Logger.Log(SFSObjectToString(obj));
            }
        }

        string SFSObjectToString(SFSObject obj)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (string key in obj.GetKeys())
                dict.Add(key, obj.GetData(key).Data);
            return JsonUtil.Serialize(dict);
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
