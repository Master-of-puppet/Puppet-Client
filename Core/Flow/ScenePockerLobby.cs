using Puppet.Core.Model;
using Puppet;
using System;
using System.Collections.Generic;

namespace Puppet.Core.Flow
{
    internal class ScenePockerLobby : BaseSingleton<ScenePockerLobby>, IScene
    {
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

        public override void Init()
        {
        }

        public void ProcessEvents(string eventType, Network.Socket.ISocketResponse onEventResponse)
        {
        }
    }
}
