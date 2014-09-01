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

namespace Puppet.Core.Network.Socket
{
    internal class CSmartFox : ISocket
    {
        public event Action<string, ISocketResponse> onResponse;
        internal SmartFox smartFox;
        ConfigData cfg;

        public CSmartFox(Action<string, ISocketResponse> onEventResponse)
        {
            ThreadHandler.QueueOnMainThread(() =>
            {
                cfg = new ConfigData();
                cfg.Host = PuMain.Setting.ServerModeSocket.Domain;
                cfg.Port = PuMain.Setting.ServerModeSocket.Port;
                cfg.Zone = PuMain.Setting.ZoneName;

                smartFox = new SmartFox(true);
                smartFox.ThreadSafeMode = false;

                if (onEventResponse != null)
                    AddListener(onEventResponse);

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

                //smartFox.AddLogListener(Sfs2X.Logging.LogLevel.INFO, OnDebugMessage);
                //smartFox.AddLogListener(Sfs2X.Logging.LogLevel.WARN, OnDebugMessage);
                //smartFox.AddLogListener(Sfs2X.Logging.LogLevel.DEBUG, OnDebugMessage);
                //smartFox.AddLogListener(Sfs2X.Logging.LogLevel.ERROR, OnDebugMessage);

                Thread thread = new Thread(new ThreadStart(() =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(0.1f));
                    smartFox.ProcessEvents();
                }));
                thread.Start();
            });
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
                smartFox.Connect(cfg);
                Logger.Log("Connecting...");
            }
        }

        public void Disconnect()
        {
            smartFox.Disconnect();
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
            ThreadHandler.QueueOnMainThread(() =>
            {
                Logger.Log("SFServer: {0}: {1}", evt.Type, MiniJSON.Json.Serialize(evt.Params));
                if (onResponse != null)
                    onResponse(evt.Type, new SFSocketResponse(evt));
            });
        }
        
        private void OnDebugMessage(BaseEvent evt)
        {
            Logger.Log("{0}: {1}", evt.Type, evt.Params["message"]);
        }
    }
}
