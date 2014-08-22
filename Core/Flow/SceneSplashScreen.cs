using Puppet.Core.Model;
using Puppet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Flow
{
    internal class SceneSplashScreen : BaseSingleton<SceneSplashScreen>, IScene
    {
        public string SceneName
        {
            get { return "SplashScreen"; }
        }

        public EScene SceneType
        {
            get { return EScene.SplashScreen; }
        }

        public IScene NextScene
        {
            get { return SceneLogin.Instance; }
        }

        public IScene PrevScene
        {
            get { return null; }
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
