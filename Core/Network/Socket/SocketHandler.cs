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

namespace Puppet.Core.Network.Socket
{
    public class SocketHandler
    {
        CSmartFox socket;
        string accessToken;

        public SocketHandler()
        {


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
                    //smartFox.Send(new LoginRequest(string.Empty, string.Empty, PuMain.Setting.ZoneName, obj));

                    ExtensionRequest request = new ExtensionRequest("test", new SFSObject());
                    //smartFox.Send(request);
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
                Logger.Log(Utility.SFSObjectToString(obj));
            }
        }
    }
}
