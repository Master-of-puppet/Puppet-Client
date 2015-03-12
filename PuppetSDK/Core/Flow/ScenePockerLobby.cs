using Puppet.Core.Model;
using Puppet;
using System;
using System.Linq;
using System.Collections.Generic;
using Puppet.Core.Network.Socket;
using Sfs2X.Entities.Data;
using Sfs2X.Core;
using Puppet.Core.Model.Datagram;
using Puppet.Utils;
using Puppet.Core.Model.Factory;

namespace Puppet.Core.Flow
{
    internal class ScenePockerLobby : BaseSingleton<ScenePockerLobby>, IScene
    {
        Dictionary<int, DataLobby> currentAllLobby;
        Dictionary<string, List<DataLobby>> currentAllChannel;

        internal DelegateAPICallback onCreateLobbyCallback;
        internal DelegateAPICallback onJoinLobbyCallback;
        internal DelegateAPICallbackObject onQuickJoinLobbyCallback;
        DelegateAPICallbackDataLobby onGetAllLobby;
        DelegateAPICallbackDataLobby onGetGroupChildrenCallback;

        internal List<DataLobby> lastUpdateGroupChildren;

        #region DEFAULT NOT MODIFY
        public string ServerScene
        {
            get { return "lobby"; }
        }

        public string SceneName
        {
            get { return "PockerLobby"; }
        }

        public EScene SceneType
        {
            get { return EScene.Pocker_Lobby; }
        }

        public IScene NextScene
        {
            get { return ScenePockerGameplay.Instance; }
        }

        public IScene PrevScene
        {
            get { return ScenePockerPlaza.Instance; }
        }
        #endregion

        public void BeginScene()
        {
            currentAllLobby = new Dictionary<int, DataLobby>();
            currentAllChannel = new Dictionary<string, List<DataLobby>>();
            RoomHandler.Instance.LoadGroupLobby(out PuGlobal.Instance.GroupsLobby);
        }

        public void EndScene()
        {
        }

        protected override void Init()
        {
        }

        public void ProcessEvents(string eventType, Network.Socket.ISocketResponse onEventResponse)
        {
            if (eventType.Equals(SFSEvent.EXTENSION_RESPONSE))
            {
                ISFSObject obj = Utility.GetDataExtensionResponse(onEventResponse, Fields.RESPONSE_CMD_PLUGIN_MESSAGE);
                if (obj != null)
                {
                    string command = Utility.GetCommandName(obj);

                    if (command == "quickJoinGame")
                    {
                        ResponseQuickJoinGame quickJoinGame = SFSDataModelFactory.CreateDataModel<ResponseQuickJoinGame>(obj);
                        DispathQuickJoinLobby(quickJoinGame);
                    }
                    else if (command == "updateChildren" || command == "updateGroupChildren")
                    {
                        ResponseListLobby responseLobby = SFSDataModelFactory.CreateDataModel<ResponseListLobby>(obj);
                        if (responseLobby.command == "updateChildren")
                            DispathGetAllLobby(true, string.Empty, new List<DataLobby>(responseLobby.children));
                        else if (responseLobby.command == "updateGroupChildren")
                        {
                            lastUpdateGroupChildren = new List<DataLobby>(responseLobby.children);
                            DispathGetGroupChildren(true, string.Empty, lastUpdateGroupChildren);
                        }
                    }
                }
            }
        }

        internal void GetAllLobby(DelegateAPICallbackDataLobby onGetAllLobby)
        {
            this.onGetAllLobby = onGetAllLobby;
            PuMain.Socket.Request(RequestPool.GetRequestGetChidren());
        }

        internal void SetSelectChannel(DataChannel channel, DelegateAPICallbackDataLobby onGetGroupChildrenCallback)
        {
            PuGlobal.Instance.SelectedChannel = channel;
            this.onGetGroupChildrenCallback = onGetGroupChildrenCallback;
            PuMain.Socket.Request(RequestPool.GetRequestGetGroupChildren(channel.name));
        }

        internal void GetGroupsLobby(DelegateAPICallbackDataChannel onGetGroupsLobbyCallback)
        {
            onGetGroupsLobbyCallback(true, string.Empty, PuGlobal.Instance.GroupsLobby);   
        }

        internal void CreateLobby(double maxBet, int numberPlayer, DelegateAPICallback onCreateLobbyCallback)
        {
            this.onCreateLobbyCallback = onCreateLobbyCallback;
            PuMain.Socket.Request(RequestPool.GetRequestCreateLobby(maxBet, numberPlayer));
        }

        internal void JoinLobby(DataLobby lobby, DelegateAPICallback onJoinLobbyCallback)
        {
            this.onJoinLobbyCallback = onJoinLobbyCallback;
            PuMain.Socket.Request(RequestPool.GetJoinRoomRequest(new RoomInfo(lobby.roomId)));
        }

        internal void QuickJoinLobby(DelegateAPICallbackObject onQuickJoinLobbyCallback)
        {
            this.onQuickJoinLobbyCallback = onQuickJoinLobbyCallback;
            PuMain.Socket.Request(RequestPool.GetQuickJoinRoomRequest());
        }

        void DispathGetAllLobby(bool status, string message, List<DataLobby> data)
        {
            if (status)
            {
                currentAllLobby.Clear();
                currentAllChannel.Clear();
                foreach (DataLobby lobby in data)
                {
                    currentAllLobby[lobby.roomId] = lobby;
                    List<DataLobby> list = (currentAllChannel.ContainsKey(lobby.groupName)) ? list = currentAllChannel[lobby.groupName] : new List<DataLobby>();
                    list.Add(lobby);
                    currentAllChannel[lobby.groupName] = list;
                }
            }
            else
                Logger.LogWarning("Can't get lobby information");

            if (onGetAllLobby != null)
                onGetAllLobby(status, message, data);
            onGetAllLobby = null;
        }

        void DispathGetGroupChildren(bool status, string message, List<DataLobby> data)
        {
            if (onGetGroupChildrenCallback != null)
                onGetGroupChildrenCallback(status, message, data);
            //onGetGroupChildrenCallback = null;
        }

        void DispathQuickJoinLobby(ResponseQuickJoinGame quickJoinGame)
        {
            if (quickJoinGame != null)
            {
                if (onQuickJoinLobbyCallback != null)
                    onQuickJoinLobbyCallback(true, quickJoinGame.message, quickJoinGame.roomId);

                if (quickJoinGame.roomId >= 0)
                    PuMain.Socket.Request(RequestPool.GetJoinRoomRequest(new RoomInfo(quickJoinGame.roomId)));
            }

            onQuickJoinLobbyCallback = null;
        }
    }
}
