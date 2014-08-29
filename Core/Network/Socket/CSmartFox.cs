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

            smartFox.AddEventListener(SFSEvent.ADMIN_MESSAGE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.CONFIG_LOAD_FAILURE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.CONFIG_LOAD_SUCCESS, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.CONNECTION, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.CONNECTION_ATTEMPT_HTTP, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.CONNECTION_LOST, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.CONNECTION_RESUME, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.CONNECTION_RETRY, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.HANDSHAKE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.INVITATION, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.INVITATION_REPLY, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.INVITATION_REPLY_ERROR, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.LOGIN, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.LOGIN_ERROR, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.LOGOUT, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.MMOITEM_VARIABLES_UPDATE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.MODERATOR_MESSAGE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.OBJECT_MESSAGE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.PING_PONG, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.PLAYER_TO_SPECTATOR, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.PLAYER_TO_SPECTATOR_ERROR, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.PRIVATE_MESSAGE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.PROXIMITY_LIST_UPDATE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.PUBLIC_MESSAGE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_ADD, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_CAPACITY_CHANGE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_CAPACITY_CHANGE_ERROR, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_CREATION_ERROR, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_FIND_RESULT, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_GROUP_SUBSCRIBE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_GROUP_SUBSCRIBE_ERROR, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_GROUP_UNSUBSCRIBE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_GROUP_UNSUBSCRIBE_ERROR, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_JOIN, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_NAME_CHANGE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_NAME_CHANGE_ERROR, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_PASSWORD_STATE_CHANGE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_PASSWORD_STATE_CHANGE_ERROR, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_REMOVE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.ROOM_VARIABLES_UPDATE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.SOCKET_ERROR, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.SPECTATOR_TO_PLAYER, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.SPECTATOR_TO_PLAYER_ERROR, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.UDP_INIT, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.USER_COUNT_CHANGE, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.USER_ENTER_ROOM, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.USER_EXIT_ROOM, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.USER_FIND_RESULT, ListenerDelegate);
            smartFox.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, ListenerDelegate);
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
                Logger.Log(Utility.SFSObjectToString(obj));
            }
        }

        public void ListenerDelegateLog(BaseEvent evt)
        {
            Logger.Log("SFLog: {0}: {1}", evt.Type, MiniJSON.Json.Serialize(evt.Params));
        }

        public void StartProcessEvent()
        {
            Thread thread = new Thread(new ThreadStart(() => 
            {
                Thread.Sleep(TimeSpan.FromSeconds(PuMain.Setting.DeltaTime));
                smartFox.ProcessEvents();
            }));
            thread.Start();
        }
    }
}
