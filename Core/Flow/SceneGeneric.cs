using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Network.Socket;
using Puppet.Utils;
using Sfs2X.Core;
using Sfs2X.Entities;

namespace Puppet.Core.Flow
{
    internal sealed class SceneGeneric : BaseSingleton<SceneGeneric>
    {
        DelegateAPICallback onBackSceneCallback;

        protected override void Init()
        {
            SocketHandler.Instance.AddListener(GenericProcessEvents);

        }

        public void Start() { }

        void GenericProcessEvents(string eventType, ISocketResponse response)
        {
            #region Back Scene
            if (onBackSceneCallback != null)
            {
                if (eventType.Equals(SFSEvent.ROOM_JOIN))
                {
                    SceneHandler.Instance.Scene_Back(RoomHandler.Instance.GetSceneNameFromCurrentRoom);
                    DispathBackScene(true, string.Empty);
                }
                else if (eventType.Equals(SFSEvent.ROOM_JOIN_ERROR))
                {
                    #warning Note: Need to localization content
                    DispathBackScene(false, "Không thể trở về màn trước!!!");
                }
            }
            #endregion

            if (eventType.Equals(SFSEvent.USER_VARIABLES_UPDATE))
            {
                UserHandler.Instance.SetCurrentUser(response);
            }
        }

        public void BackScene(DelegateAPICallback onBackSceneCallback)
        {
            this.onBackSceneCallback = onBackSceneCallback;
            PuMain.Socket.Request(RequestPool.GetJoinRoomRequest(RoomHandler.Instance.GetParentRoom));
        }

        void DispathBackScene(bool status, string message)
        {
            if (onBackSceneCallback != null)
                onBackSceneCallback(status, message);
            onBackSceneCallback = null;
        }
    }
}
