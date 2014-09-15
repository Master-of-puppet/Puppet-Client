using Puppet.Core.Model;
using Puppet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Network.Socket;
using Sfs2X.Core;
using Puppet.Utils;
using Sfs2X.Entities.Data;
using Puppet.Core.Model.Datagram;

namespace Puppet.Core.Flow
{
    internal class SceneWorldGame : BaseSingleton<SceneWorldGame>, IScene
    {
        ResponseListGame responseGame;

        DelegateAPICallbackDataGame onGetListGame;
        DelegateAPICallback onJoinRoomCallback;

        #region DEFAULT NOT MODIFY
        public string SceneName
        {
            get { return "WorldGame"; }
        }

        public EScene SceneType
        {
            get { return EScene.World_Game; }
        }

        public IScene NextScene
        {
            get { return ScenePockerPlaza.Instance; }
        }

        public IScene PrevScene
        {
            get { return SceneLogin.Instance; }
        }
        #endregion

        public void BeginScene()
        {
        }

        public void EndScene()
        {
        }

        protected override void Init()
        {
        }

        public void ProcessEvents(string eventType, Network.Socket.ISocketResponse onEventResponse)
        {
            if(eventType.Equals(SFSEvent.EXTENSION_RESPONSE))
            {
                string cmd = onEventResponse.Params[Fields.CMD].ToString();
                if(cmd == Fields.RESPONSE_CMD_PLUGIN_MESSAGE)
                {
                    SFSObject data = (SFSObject)onEventResponse.Params[Fields.PARAMS];
                    Logger.Log(data.GetDump(true));
                    ISFSObject obj = data.GetSFSObject(Fields.MESSAGE);
                    responseGame = Puppet.Core.Model.Factory.SFSDataModelFactory.CreateDataModel<ResponseListGame>(obj);
                    DispathGetListGame(true, string.Empty);
                }
            }
            else if(eventType.Equals(SFSEvent.ROOM_JOIN))
            {
                SceneHandler.Instance.Scene_Next(RoomHandler.Instance.GetSceneNameFromCurrentRoom);
                DispathJoinRoom(true, string.Empty);
            }
            else if(eventType.Equals(SFSEvent.ROOM_JOIN_ERROR))
            {
                #warning Note: Need to localization content
                DispathJoinRoom(false, "Không thể tham vào trò chơi " + PuGlobal.Instance.SelectedGame.roomName + "!!!!!");
            }
        }

        internal void GetListGame(DelegateAPICallbackDataGame onGetListGame)
        {
            this.onGetListGame = onGetListGame;
            PuMain.Socket.Request(RequestPool.GetRequestGetChidren());
        }

        void DispathGetListGame(bool status, string message)
        {
            if(onGetListGame != null)
                onGetListGame(status, message, new List<DataGame>(responseGame.children));
            onGetListGame = null;
        }

        internal void JoinGame(DataGame game, DelegateAPICallback onJoinRoomCallback)
        {
            PuGlobal.Instance.SelectedGame = game;
            this.onJoinRoomCallback = onJoinRoomCallback;
            PuMain.Socket.Request(RequestPool.GetJoinRoomRequest(new RoomInfo(game.roomId)));
        }

        void DispathJoinRoom(bool status, string message)
        {
            if (onJoinRoomCallback != null)
                onJoinRoomCallback(status, message);
            onJoinRoomCallback = null;
        }
    }
}
