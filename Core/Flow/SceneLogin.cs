﻿using Puppet.Core.Model;
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
                return ScenePockerPlaza.Instance;
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
                    Logger.Log("LoginRequest Sending...");
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
                Logger.Log(Utility.SFSObjectToString(obj));

                RoomInfo room = Utility.GetDataFromResponse<RoomInfo>(response, Fields.DATA, Fields.RESPONSE_FIRST_ROOM_TO_JOIN);


                PuMain.Socket.Request(RequestPool.GetJoinRoomRequest(room));
                Logger.Log("JoinRoomRequest Sending... {0}", room.ToString());

                //ExtensionRequest request = new ExtensionRequest("test", new SFSObject());
                //PuMain.Socket.Request(new SFSocketRequest(request));
            }
            else if (eventType.Equals(SFSEvent.LOGIN_ERROR))
            {
                #warning Note: Need to localization content
                DispathEventLogin(false, "Thông tin đăng nhập không hợp lệ");
            }
            else if(eventType.Equals(SFSEvent.EXTENSION_RESPONSE))
            {
                Logger.Log(response.Type);

                DispathEventLogin(true, string.Empty);
                SceneHandler.Instance.Scene_Next();
            }
        }

        void DispathEventLogin(bool status, string message)
        {
            if (onLoginCallback != null)
                onLoginCallback(status, message);
        }
    }
}
