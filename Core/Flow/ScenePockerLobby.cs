using Puppet.Core.Model;
using Puppet;
using System;
using System.Linq;
using System.Collections.Generic;
using Puppet.Core.Network.Socket;
using Sfs2X.Entities.Data;
using Sfs2X.Core;
using Puppet.Core.Model.Datagram;

namespace Puppet.Core.Flow
{
    internal class ScenePockerLobby : BaseSingleton<ScenePockerLobby>, IScene
    {
        internal List<DataChannel> GroupsLobby;
        internal DataChannel selectedChannel;

        Dictionary<int, DataLobby> currentAllLobby;
        Dictionary<string, List<DataLobby>> currentAllChannel;

        DelegateAPICallbackDataLobby onGetAllLobby;
        DelegateAPICallbackDataLobby onGetGroupChildrenCallback;
        DelegateAPICallbackDataLobby onCreateLobbyCallback;

        #region DEFAULT NOT MODIFY
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
            RoomHandler.Instance.LoadGroupLobby(out GroupsLobby);
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
                string cmd = onEventResponse.Params[Fields.CMD].ToString();
                if (cmd == Fields.RESPONSE_CMD_PLUGIN_MESSAGE)
                {
                    SFSObject data = (SFSObject)onEventResponse.Params[Fields.PARAMS];
                    Logger.Log(data.GetDump());
                    ISFSObject obj = data.GetSFSObject(Fields.MESSAGE);

                    ResponseListLobby responseLobby = Puppet.Core.Model.Factory.SFSDataModelFactory.CreateDataModel<ResponseListLobby>(obj);
                    if (responseLobby.command == "updateChildren")
                        DispathGetAllLobby(true, string.Empty, new List<DataLobby>(responseLobby.children));
                    else if (responseLobby.command == "updateGroupChildren")
                        DispathGetGroupChildren(true, string.Empty, new List<DataLobby>(responseLobby.children));
                }
            }
            else if (eventType.Equals(SFSEvent.ROOM_JOIN))
            {
                SceneHandler.Instance.Scene_Next(RoomHandler.Instance.GetSceneNameFromCurrentRoom);
                DispathCreateGame(true, string.Empty);
            }
            else if (eventType.Equals(SFSEvent.ROOM_JOIN_ERROR))
            {
                #warning Note: Need to localization content
                DispathCreateGame(false, "Không thể tạo bàn chơi.!!!!");
            }
        }

        internal void GetAllLobby(DelegateAPICallbackDataLobby onGetAllLobby)
        {
            this.onGetAllLobby = onGetAllLobby;
            PuMain.Socket.Request(RequestPool.GetRequestGetChidren());
        }

        internal void SetSelectChannel(DataChannel channel, DelegateAPICallbackDataLobby onGetGroupChildrenCallback)
        {
            selectedChannel = channel;
            this.onGetGroupChildrenCallback = onGetGroupChildrenCallback;
            PuMain.Socket.Request(RequestPool.GetRequestGetGroupChildren(selectedChannel.name));
        }

        internal void GetGroupsLobby(DelegateAPICallbackDataChannel onGetGroupsLobbyCallback)
        {
            onGetGroupsLobbyCallback(true, string.Empty, GroupsLobby);   
        }

        internal void CreateLobby(DelegateAPICallbackDataLobby onCreateLobbyCallback)
        {
            this.onCreateLobbyCallback = onCreateLobbyCallback;
            PuMain.Socket.Request(RequestPool.GetRequestCreateLobby());
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
            onGetGroupChildrenCallback = null;
        }

        void DispathCreateGame(bool status, string message)
        {
            if (onCreateLobbyCallback != null)
                onCreateLobbyCallback(status, message, null);
            onCreateLobbyCallback = null;
        }
    }
}
