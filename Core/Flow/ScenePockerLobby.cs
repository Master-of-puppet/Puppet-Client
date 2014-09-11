using Puppet.Core.Model;
using Puppet;
using System;
using System.Collections.Generic;
using Puppet.Core.Network.Socket;
using Sfs2X.Entities.Data;
using Sfs2X.Core;
using Puppet.Core.Model.Datagram;

namespace Puppet.Core.Flow
{
    internal class ScenePockerLobby : BaseSingleton<ScenePockerLobby>, IScene
    {
        ResponseListChannel responseChannel;

        Action<bool, string, List<DataChannel>> onGetListChannel;
        Action<bool, string, List<DataChannel>> onGetGroupCallback;

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
                    Logger.Log(data.GetDump(true));
                    ISFSObject obj = data.GetSFSObject(Fields.MESSAGE);
                    responseChannel = Puppet.Core.Model.Factory.SFSDataModelFactory.CreateDataModel<ResponseListChannel>(obj);
                    DispathGetListChannel(true, string.Empty);
                }
            }
        }

        internal void GetListChannel(Action<bool, string, List<DataChannel>> onGetListChannel)
        {
            this.onGetListChannel = onGetListChannel;
            PuMain.Socket.Request(RequestPool.GetRequestGetChidren());
        }

        internal void GetGroupChildren(string groupName, Action<bool, string, List<DataChannel>> onGetGroupCallback)
        {
            this.onGetGroupCallback = onGetGroupCallback;
            PuMain.Socket.Request(RequestPool.GetRequestGetGroupChildren(groupName));
        }

        void DispathGetListChannel(bool status, string message)
        {
            if (onGetListChannel != null)
                onGetListChannel(status, message, new List<DataChannel>(responseChannel.children));
            onGetListChannel = null;
        }

        void DispathGetGroup(bool status, string message, List<DataChannel> data)
        {
            if (onGetGroupCallback != null)
                onGetGroupCallback(status, message, data);
            onGetGroupCallback = null;
        }
    }
}
