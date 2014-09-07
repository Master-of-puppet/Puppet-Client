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

namespace Puppet.Core.Flow
{
    internal class SceneLogin : BaseSingleton<SceneLogin>, IScene
    {
        string token;
        Action<bool, string> onLoginCallback;

        #region DEFAULT NOT MODIFY
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

        public void Login(string token, Action<bool, string> onLoginCallback)
        {
            this.token = token;
            this.onLoginCallback = onLoginCallback;

            PuMain.Socket.Connect();
        }

        public void ProcessEvents(string eventType, ISocketResponse response)
        {
            if (eventType.Equals(SFSEvent.CONNECTION))
            {
                bool success = (bool)response.Params[Fields.SUCCESS];
                if (success)
                {
                    PuMain.Socket.Request(RequestPool.GetLoginRequest(token));
                }
                else
                {
                    #warning Note: Need to localization content
                    DispathEventLogin(false, "Không thể kết nối đến máy chủ");
                }
            }
            else if (eventType.Equals(SFSEvent.LOGIN))
            {
                SFSObject obj = (SFSObject)response.Params[Fields.DATA];
                RoomInfo room = Utility.GetDataFromResponse<RoomInfo>(response, Fields.DATA, Fields.RESPONSE_FIRST_ROOM_TO_JOIN);
                PuMain.Socket.Request(RequestPool.GetJoinRoomRequest(room));
            }
            else if (eventType.Equals(SFSEvent.LOGIN_ERROR))
            {
                #warning Note: Need to localization content
                DispathEventLogin(false, "Thông tin đăng nhập không hợp lệ");
            }
            else if (eventType.Equals(SFSEvent.ROOM_JOIN_ERROR))
            {
                #warning Note: Need to localization content
                DispathEventLogin(false, "Đăng nhập thành công, nhưng không thể tham gia vào phòng chơi.");
            }
            else if(eventType.Equals(SFSEvent.ROOM_JOIN))
            {
                SceneHandler.Instance.Scene_Next(RoomHandler.Instance.GetSceneNameFromCurrentRoom);
                DispathEventLogin(true, string.Empty);
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
