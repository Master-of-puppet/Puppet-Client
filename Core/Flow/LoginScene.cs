using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Flow
{
    class LoginScene : IScene
    {

        public EScene SceneType
        {
            get { return EScene.LoginScreen; }
        }

        public string SceneName
        {
            get { return "LoginScreen"; }
        }

        public IScene NextScene
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IScene PrevScene
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void EnterScene()
        {
            throw new NotImplementedException();
        }


        public IScene OldScene
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void BeginScene()
        {
            throw new NotImplementedException();
        }

        public void EndScene()
        {
            throw new NotImplementedException();
        }
    }
}
