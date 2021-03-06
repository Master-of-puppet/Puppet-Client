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
        /// <summary>
        /// Thông báo server quay về cảnh trước đó, Bao gồm cả Logout.
        /// </summary>
        public static void BackScene(DelegateAPICallback onBackSceneCallback)
        {
            SceneGeneric.Instance.BackScene(onBackSceneCallback);
        }

        /// <summary>
        /// Thông báo cần đăng xuất.
        /// </summary>
        /// <param name="onLogoutCallback"></param>
        public static void LoginOut(DelegateAPICallback onLogoutCallback)
        {
            SceneGeneric.Instance.LoginOut(onLogoutCallback);
        }

        /// <summary>
        /// Khi request server timeout, tạo lại các kết nối.
        /// </summary>
        public static void RequestTimeOut()
        {
            SceneGeneric.Instance.RequestTimeOut();
        }

        /// <summary>
        /// Nhận phần thưởng hàng ngày.
        /// Cần implemnent event EventDispatcher.onDailyGift.
        /// </summary>
        public static void GetDailyGift()
        {
            SceneGeneric.Instance.GetDailyGift();
        }

        /// <summary>
        /// Yêu cầu lấy danh sách các sự kiện.
        /// </summary>
        public static void GetInfoEvents(Action<bool, string, DataResponseEvents> callback)
        {
            HttpPool.GetInfoEvents(callback);
        }

        /// <summary>
        /// Lấy thông tin về các thể loại nạp tiền.
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
        public static void RechargeCard(string pin, string serial, string trackId, string type, DelegateAPICallback callback)
        {
            HttpPool.RechargeCard(APIUser.USERNAME, pin, serial, trackId, type, callback);
        }

        /// <summary>
        /// Đăng mẩu tin lên Facebook
        /// </summary>
        /// <param name="accessToken">Facebook AccessToken</param>
        /// <param name="title">Tiêu đề sẽ được đăng</param>
        /// <param name="picture">Hình ảnh sẽ được đăng</param>
        public static void PostFacebook(string accessToken, string title, byte[] picture, DelegateAPICallback callback)
        {
            HttpPool.PostFacebook(APIUser.USERNAME, accessToken, title, picture, callback);
        }

        /// <summary>
        /// Thông báo để server lưu lại các yêu cầu đã gửi.
        /// (Được dùng sau khi Request Application https://developers.facebook.com/docs/unity/reference/current/FB.Apprequest)
        /// </summary>
        /// <param name="facebookId">Facebook Id của bạn</param>
        /// <param name="requestIds">Danh sách Ids các người chơi được gửi yêu cầu</param>
        public static void SaveRequestFB(string facebookId, string[] requestIds, DelegateAPICallback callback)
        {
            HttpPool.SaveRequestFB(facebookId, APIUser.USER_ID.ToString(), requestIds, callback);
        }

        /// <summary>
        /// Lấy danh sách các avatar mặc định từ server.
        /// </summary>
        public static void GetDefaultAvatar(DelegateAPICallbackListString callback)
        {
            HttpPool.GetDefaultAvatar(4, callback);
        }

        /// <summary>
        /// Thay đổi avatar của người chơi sử dụng avatar mặc định.
        /// </summary>
        public static void SaveDefaultAvatar(string path, DelegateAPICallback callback)
        {
            HttpPool.SaveDefaultAvatar(APIUser.USERNAME, path, callback);
        }

        /// <summary>
        /// Lấy thông tin các người chơi có trong phòng hiện tại.
        /// </summary>
        /// <returns>Danh sách các người chơi (bao gồm cả người đang gọi API)</returns>
        public static List<UserInfo> GetUsersInRoom()
        {
            return RoomHandler.Instance.GetUsersInRoom();
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

        /// <summary>
        /// Lấy thông tin về cấu hình ứng dụng của người chơi (như âm thanh, các tùy chọn...)
        /// </summary>
        /// <param name="version">Và thông tin về phiên bản hiện tại</param>
        public static PuGameOption GetOptionInfo(out string version)
        {
            version = PuMain.Setting.ClientDetails.version.ToString();
            return PuSession.Option;
        }

        /// <summary>
        /// Lưu lại thay đổi của người chơi
        /// </summary>
        public static void ChangeOptionInfo(PuGameOption newOption)
        {
            PuSession.Option = newOption;
        }

        /// <summary>
        /// Thông tin về cấu hình app được lưu lại server
        /// </summary>
        public static ResponseAppConfig GetAppConfig
        {
            get { return PuGlobal.Instance.currentAppConfig; }
        }
    }
}
