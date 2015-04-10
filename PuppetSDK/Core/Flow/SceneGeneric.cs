using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Network.Socket;
using Puppet.Utils;
using Sfs2X.Core;
using Sfs2X.Entities;
using Puppet.Core.Model;
using Sfs2X.Entities.Data;
using Puppet.Core.Model.Factory;

namespace Puppet.Core.Flow
{
    internal sealed class SceneGeneric : BaseSingleton<SceneGeneric>
    {
        DelegateAPICallback onBackSceneCallback;
        DelegateAPICallback onLogoutCallback;

        protected override void Init() {}

        public void StartListenerEvent() 
        {
            SocketHandler.Instance.AddListener(GenericProcessEvents);
        }

        void GenericProcessEvents(string eventType, ISocketResponse response)
        {
            if (eventType.Equals(SFSEvent.LOGIN))
            {
                SFSObject obj = (SFSObject)response.Params[Fields.DATA];
                RoomInfo firtRoomToJoin = Utility.GetDataFromResponse<RoomInfo>(response, Fields.DATA, Fields.RESPONSE_FIRST_ROOM_TO_JOIN);
                PuGlobal.Instance.FirtRoomToJoin = firtRoomToJoin;
                PuMain.Socket.Request(RequestPool.GetJoinRoomRequest(firtRoomToJoin));
            }
            else if (eventType.Equals(SFSEvent.CONNECTION))
            {
                bool success = (bool)response.Params[Fields.SUCCESS];
                if (success)
                    PuMain.Socket.Request(RequestPool.GetLoginRequest(PuGlobal.Instance.token));
            }
            else if (eventType.Equals(SFSEvent.CONNECTION_LOST))
            {
                string reasonLostConnect = (string)response.Params["reason"];
                Logger.Log(ELogColor.CYAN, "Lost Connection Reason: {0}", reasonLostConnect);
                if (reasonLostConnect == "idle")
                {
                    PuMain.Socket.Reconnect();
                    PuMain.Dispatcher.SetNoticeMessage(EMessage.Message, "Đăng cố kết nối lại với máy chủ.");
                }
                else
                {
                    DisconnectAndLogin();
                    PuMain.Dispatcher.SetNoticeMessage(EMessage.Message, "Mất kết nối đến máy chủ. Vui lòng đăng nhập lại.");
                }
            }
            else if (eventType.Equals(SFSEvent.EXTENSION_RESPONSE))
            {
                ISFSObject obj = Utility.GetGlobalExtensionResponse(response, Fields.RESPONSE_CMD_GLOBAL_MESSAGE);
                if (obj != null)
                {
                    string command = Utility.GetCommandName(obj);
                    if (command == "openDailyGift")
                    {
                        DataDailyGift dataGift = SFSDataModelFactory.CreateDataModel<DataDailyGift>(obj);
                        PuGlobal.Instance.CurrentDailyGift = dataGift;
                        PuMain.Dispatcher.SetDailyGift(dataGift);
                    }
                    else if(command == "customMessage")
                    {
                        DataCustomMessage dataMessage = SFSDataModelFactory.CreateDataModel<DataCustomMessage>(obj);
                        PuMain.Dispatcher.SetNoticeMessage(EMessage.Message, dataMessage.content);
                    }
                }
            }
            else if (eventType.Equals(SFSEvent.ROOM_JOIN))
            {
                //if (onBackSceneCallback != null) SceneHandler.Instance.Scene_Back(RoomHandler.Instance.GetSceneNameFromCurrentRoom);
                //else SceneHandler.Instance.Scene_Next(RoomHandler.Instance.GetSceneNameFromCurrentRoom);
                SceneHandler.Instance.Scene_GoTo(RoomHandler.Instance.GetSceneNameFromCurrentRoom);

                DispathAPICallback(ref onBackSceneCallback, true, string.Empty);
                DispathAPICallback(ref SceneLogin.Instance.onLoginCallback, true, string.Empty);
                DispathAPICallback(ref SceneWorldGame.Instance.onJoinGameCallback, true, string.Empty);
                DispathAPICallback(ref ScenePockerLobby.Instance.onCreateLobbyCallback, true, string.Empty);
                DispathAPICallback(ref ScenePockerLobby.Instance.onJoinLobbyCallback, true, string.Empty);
            }
            else if (eventType.Equals(SFSEvent.ROOM_JOIN_ERROR))
            {
                #warning Note: Need to localization content
                DispathAPICallback(ref onBackSceneCallback, false, "Không thể trở về màn trước!!!");
                DispathAPICallback(ref SceneLogin.Instance.onLoginCallback, false, "Đăng nhập thành công, nhưng không thể tham gia vào phòng chơi.");
                DispathAPICallback(ref SceneWorldGame.Instance.onJoinGameCallback, false, "Không thể tham vào trò chơi!!!");
                DispathAPICallback(ref ScenePockerLobby.Instance.onCreateLobbyCallback, false, "Không thể tạo bàn chơi.!!!!");
                DispathAPICallback(ref ScenePockerLobby.Instance.onJoinLobbyCallback, false, "Không thể tham gia vào bàn chơi.!!!!");
            }
            else if (eventType.Equals(SFSEvent.USER_VARIABLES_UPDATE))
            {
                UserHandler.Instance.SetCurrentUser(response);
            }
            else if (eventType.Equals(SFSEvent.LOGOUT))
            {
                DispathAPICallback(ref onLogoutCallback, true, string.Empty);
                DisconnectAndLogin();
            }
            else if (eventType.Equals(SFSEvent.PING_PONG))
            {

            }
            else if (eventType.Equals(SFSEvent.PUBLIC_MESSAGE))
            {
                if (response.Params.Contains(Fields.MESSAGE) && response.Params[Fields.MESSAGE].ToString() == DefineKeys.KEY_CHAT_MESSAGE)
                {
                    ISFSObject obj = (SFSObject)response.Params[Fields.DATA];
                    DataChat dataChat = SFSDataModelFactory.CreateDataModel<DataChat>(obj);
                    PuMain.Dispatcher.SetChatMessage(dataChat);
                }
            }
        }

        private void DisconnectAndLogin()
        {
            SceneHandler.Instance.Scene_GoTo(EScene.LoginScreen, string.Empty);
            PuMain.Socket.Disconnect();

            UserHandler.Instance.ResetSingleton();
            RoomHandler.Instance.ResetSingleton();
            PuGlobal.Instance.ResetSingleton();
        }

        internal void GetDailyGift()
        {
            if (PuGlobal.Instance.CurrentDailyGift != null)
                PuMain.Socket.Request(RequestPool.GetDailyGift(PuGlobal.Instance.CurrentDailyGift));
        }

        internal void SendPublicMessage(string message, AbstractData data = null)
        {
            PuMain.Socket.Request(RequestPool.GetPublicMessageRequest(message, data));
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

        internal void LoginOut(DelegateAPICallback onLogoutCallback)
        {
            PuSession.Login.Clear();
            this.onLogoutCallback = onLogoutCallback;
            PuMain.Socket.Request(RequestPool.GetLogout());
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
