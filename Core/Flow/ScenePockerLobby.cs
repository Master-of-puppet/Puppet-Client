using Puppet.Core.Model;
using Puppet;
using System;
using System.Collections.Generic;

namespace Puppet.Core.Flow
{
    internal class ScenePockerLobby : BaseSingleton<ScenePockerLobby>, IScene
    {
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

        public void BeginScene()
        {
        }

        public void EndScene()
        {
        }

        public override void Init()
        {
        }
    }
}
