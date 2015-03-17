﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Flow;
using Puppet.Core.Network.Http;
using Puppet.Core.Model;
using Puppet.Core;
using Puppet.Core.Network.Socket;

namespace Puppet.API.Client
{
    public sealed class APIGeneric
    {
        private static string USERNAME
        {
            get { return PuGlobal.Instance.mUserInfo.info.userName; }
        }

        /// <summary>
        /// Thông báo server quay về cảnh trước đó, Bao gồm cả Logout.
        /// </summary>
        public static void BackScene(DelegateAPICallback onBackSceneCallback)
        {
            SceneGeneric.Instance.BackScene(onBackSceneCallback);
        }

        public static void LoginOut(DelegateAPICallback onLogoutCallback)
        {
            SceneGeneric.Instance.LoginOut(onLogoutCallback);
        }

        /// <summary>
        /// Nhận phần thưởng hàng ngày
        /// </summary>
        public static void GetDailyGift()
        {
            SceneGeneric.Instance.GetDailyGift();
        }

        /// <summary>
        /// Lấy thông tin về các thể loại nạp tiền
        /// </summary>
        public static void GetInfoRecharge(Action<bool, string, DataResponseRecharge> callback)
        {
            HttpPool.GetInfoRecharge(callback);
        }

        /// <summary>
        /// Nạp tiền cho hệ thông (sử dụng thẻ cào)
        /// </summary>
        /// <param name="pin">Mã pin của thẻ sẽ nạp</param>
        /// <param name="serial">Mã serial của thẻ sẽ nạp</param>
        /// <param name="type">Loại thẻ là một trong các loại 'VIETTEL'; 'VINAPHONE'; 'MOBIFONE'; 'VCOIN'; 'GATE'; 'MEGACARD'</param>
        public static void RechargeCard(string pin, string serial, string type, DelegateAPICallback callback)
        {
            HttpPool.RechargeCard(USERNAME, pin, serial, type, callback);
        }

        /// <summary>
        /// Đăng mẩu tin lên Facebook
        /// </summary>
        /// <param name="accessToken">Facebook AccessToken</param>
        /// <param name="title">Tiêu đề sẽ được đăng</param>
        /// <param name="picture">Hình ảnh sẽ được đăng</param>
        public static void PostFacebook(string accessToken, string title, byte[] picture, DelegateAPICallback callback)
        {
            HttpPool.PostFacebook(USERNAME, accessToken, title, picture, callback);
        }

        /// <summary>
        /// Lấy thông tin về RoomInfo hiện tại
        /// </summary>
        public static RoomInfo SelectedRoomJoin()
        {
            return PuGlobal.Instance.SelectedRoomJoin;
        }

        /// <summary>
        /// Gửi một message đến tất cả những người chơi cùng Room
        /// </summary>
        /// <param name="message">Nội dung</param>
        /// <param name="data">Dữ liệu truyền đi kèm</param>
        public static void SendPublicMessage(string message, AbstractData data = null)
        {
            SceneGeneric.Instance.SendPublicMessage(message, data);
        }

        /// <summary>
        /// Gửi nội dung chat đến người chơi cùng trong phòng.
        /// </summary>
        /// <param name="data">Kiểu dữ liệu sẽ gửi đi và nội dung sẽ gửi đi</param>
        public static void SendChat(DataChat data)
        {
            PuMain.Socket.Request(RequestPool.GetPublicMessageRequest(DefineKeys.KEY_CHAT_MESSAGE, data));
        }
    }
}
