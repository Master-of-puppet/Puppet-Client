using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using Puppet;
using Sfs2X.Core;
using Puppet.Core.Network.Socket;
using Sfs2X.Entities.Data;
using Puppet.Utils;
using Puppet.Core.Model.Datagram;
using Sfs2X.Requests;
using Sfs2X.Entities;

namespace Puppet.Core.Flow
{
    internal class SceneLogin : BaseSingleton<SceneLogin>, IScene
    {
        internal DelegateAPICallback onLoginCallback;

        #region DEFAULT NOT MODIFY
        public string ServerScene
        {
            get { return string.Empty; }
        }

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
                return SceneWorldGame.Instance;
            }
        }

        public IScene PrevScene
        {
            get
            {
                return null;
            }
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

        internal void Login(string token, DelegateAPICallback onLoginCallback)
        {
            PuSession.Login.SaveLoginInfo(token);

            PuGlobal.Instance.token = token;
            this.onLoginCallback = onLoginCallback;

            PuMain.Socket.Connect();
        }

        public void ProcessEvents(string eventType, ISocketResponse response)
        {
            if (eventType.Equals(SFSEvent.CONNECTION))
            {
                bool success = (bool)response.Params[Fields.SUCCESS];
                if (!success)
                {
                    #warning Note: Need to localization content
                    DispathEventLogin(false, "Không thể kết nối đến máy chủ");
                }
            }
            else if (eventType.Equals(SFSEvent.LOGIN_ERROR))
            {
                #warning Note: Need to localization content
                DispathEventLogin(false, "Thông tin đăng nhập không hợp lệ");
            }
        }

        void DispathEventLogin(bool status, string message)
        {
            if (onLoginCallback != null)
                onLoginCallback(status, message);
            onLoginCallback = null;
        }
    }
}
