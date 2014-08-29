using Puppet.Core.Model;
using Puppet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Flow
{
    internal class ScenePockerGameplay : BaseSingleton<ScenePockerGameplay>, IScene
    {
        #region DEFAULT NOT MODIFY
        public string SceneName
        {
            get { return "PockerGameplay"; }
        }

        public EScene SceneType
        {
            get { return EScene.Pocker_Gameplay; }
        }

        public IScene NextScene
        {
            get { return ScenePockerLobby.Instance; }
        }

        public IScene PrevScene
        {
            get { return ScenePockerLobby.Instance; }
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
