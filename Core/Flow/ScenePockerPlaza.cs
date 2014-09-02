using Puppet.Core.Model;
using Puppet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Flow
{
    internal class ScenePockerPlaza : BaseSingleton<ScenePockerPlaza>, IScene
    {
        #region DEFAULT NOT MODIFY
        public string SceneName
        {
            get { return "PockerPlaza"; }
        }

        public EScene SceneType
        {
            get { return EScene.Pocker_Plaza; }
        }

        public IScene NextScene
        {
            get { return ScenePockerLobby.Instance; }
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
        }
    }
}
