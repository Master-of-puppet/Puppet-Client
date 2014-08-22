using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using Puppet;

namespace Puppet.Core.Flow
{
    internal class SceneLogin : BaseSingleton<SceneLogin>, IScene
    {
        public string SceneName
        {
            get { return "LoginScreen"; }
        }

        public EScene SceneType
        {
            get { return EScene.LoginScreen; }
        }

        public IScene NextScene
        {
            get
            {
                return ScenePockerPlaza.Instance;
            }
        }

        public IScene PrevScene
        {
            get
            {
                return SceneSplashScreen.Instance;
            }
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
