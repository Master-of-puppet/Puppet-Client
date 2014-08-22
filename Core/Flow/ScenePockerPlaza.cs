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
