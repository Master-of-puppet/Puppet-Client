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
        internal DelegateAPICallback onJoinGameCallback;

        ResponseListGame responseGame;
        DelegateAPICallbackDataGame onGetListGame;

        #region DEFAULT NOT MODIFY
        public string ServerScene
        {
            get { return "games"; }
        }

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
            GetListGame((status, message, data) =>
            {
                if(status && data.Count >= 1)
                {
                    JoinGame(data[0], (joinStatus, joinMessage) =>
                    {
                    });
                }
            });
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
                ISFSObject obj = Utility.GetDataExtensionResponse(onEventResponse, Fields.RESPONSE_CMD_PLUGIN_MESSAGE);
                if(obj != null)
                {
                    responseGame = Puppet.Core.Model.Factory.SFSDataModelFactory.CreateDataModel<ResponseListGame>(obj);
                    DispathGetListGame(true, string.Empty);
                }
            }
        }

        internal void GetListGame(DelegateAPICallbackDataGame onGetListGame)
        {
            this.onGetListGame = onGetListGame;
            PuMain.Socket.Request(RequestPool.GetRequestGetChidren());
        }

        internal void JoinGame(DataGame game, DelegateAPICallback onJoinGameCallback)
        {
            PuGlobal.Instance.SelectedGame = game;
            this.onJoinGameCallback = onJoinGameCallback;
            PuMain.Socket.Request(RequestPool.GetJoinRoomRequest(new RoomInfo(game.roomId)));
        }

        void DispathGetListGame(bool status, string message)
        {
            if(onGetListGame != null)
                onGetListGame(status, message, new List<DataGame>(responseGame.children));
            onGetListGame = null;
        }
    }
}
