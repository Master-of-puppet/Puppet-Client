using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Network.Socket;
using Puppet.Utils;
using Sfs2X.Core;
using Sfs2X.Entities;
using Puppet.Core.Model;

namespace Puppet.Core.Flow
{
    internal sealed class SceneGeneric : BaseSingleton<SceneGeneric>
    {
        DelegateAPICallback onBackSceneCallback;

        protected override void Init() {}

        public void StartListenerEvent() 
        {
            SocketHandler.Instance.AddListener(GenericProcessEvents);
        }

        void GenericProcessEvents(string eventType, ISocketResponse response)
        {
            if (eventType.Equals(SFSEvent.ROOM_JOIN))
            {
                //if (onBackSceneCallback != null) SceneHandler.Instance.Scene_Back(RoomHandler.Instance.GetSceneNameFromCurrentRoom);
                //else SceneHandler.Instance.Scene_Next(RoomHandler.Instance.GetSceneNameFromCurrentRoom);
                SceneHandler.Instance.Scene_GoTo(RoomHandler.Instance.GetSceneNameFromCurrentRoom);

                DispathAPICallback(ref onBackSceneCallback, true, string.Empty);
                DispathAPICallback(ref SceneLogin.Instance.onLoginCallback, true, string.Empty);
                DispathAPICallback(ref SceneWorldGame.Instance.onJoinGameCallback, true, string.Empty);
                DispathAPICallback(ref ScenePockerLobby.Instance.onCreateLobbyCallback, true, string.Empty);
            }
            else if (eventType.Equals(SFSEvent.ROOM_JOIN_ERROR))
            {
                #warning Note: Need to localization content
                DispathAPICallback(ref onBackSceneCallback, false, "Không thể trở về màn trước!!!");
                DispathAPICallback(ref SceneLogin.Instance.onLoginCallback, false, "Đăng nhập thành công, nhưng không thể tham gia vào phòng chơi.");
                DispathAPICallback(ref SceneWorldGame.Instance.onJoinGameCallback, false, "Không thể tham vào trò chơi!!!");
                DispathAPICallback(ref ScenePockerLobby.Instance.onCreateLobbyCallback, false, "Không thể tạo bàn chơi.!!!!");
            }
            else if (eventType.Equals(SFSEvent.USER_VARIABLES_UPDATE))
            {
                UserHandler.Instance.SetCurrentUser(response);
            }
        }

        #region When Click Back UIButton
        internal void BackScene(DelegateAPICallback onBackSceneCallback)
        {
            this.onBackSceneCallback = onBackSceneCallback;
            if (RoomHandler.Instance.Current.Id == PuGlobal.Instance.FirtRoomToJoin.roomId)
                PuMain.Socket.Disconnect();
            else
                PuMain.Socket.Request(RequestPool.GetJoinRoomRequest(RoomHandler.Instance.GetParentRoom));
        }
        #endregion

        void DispathAPICallback(ref DelegateAPICallback callbackMethod, bool status, string message)
        {
            if (callbackMethod != null)
            {
                callbackMethod(status, message);
                callbackMethod = null;
            }
        }
    }
}
