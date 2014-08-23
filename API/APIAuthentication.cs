using Puppet.Core.Network.Http;
using System;
using System.Collections.Generic;
using Puppet.Utils;

namespace Puppet.API
{
    public class APIAuthentication
    {
        public static void Login(string token, Action<bool, string> onLoginCallback)
        {
            onLoginCallback(false, "Thông tin đăng nhập không hợp lệ.");
        }

        public static void SocialLogin(string socialType, string accessToken, Action<bool, string> onLoginCallback)
        {
            onLoginCallback(false, "AccessToken đã hết hạn.");
        }

        public static void Register(Dictionary<string, string> registerInformation, Action<bool, string> onRegisterCallback)
        {
            onRegisterCallback(false, "Địa chỉ email đã tồn tại trong hệ thống.");
        }

        /// <summary>
        /// Lấy thông tin về AccessToken.
        /// </summary>
        /// <param name="userName">Tên truy cập</param>
        /// <param name="password">Mật khẩu</param>
        /// <param name="onGetTokenCallback">
        /// IHttpResponse: Phản hồi từ HTTP.
        /// bool: Kết quả thành công hay thất bại.
        /// string: Token lấy được nếu thành công.
        /// </param>
        public static void GetAccessToken(string userName, string password, Action<IHttpResponse, bool, string> onGetTokenCallback)
        {
            SimpleHttpRequest request = new SimpleHttpRequest(Commands.GET_ACCESS_TOKEN, Fields.USERNAME, userName, Fields.PASSWORD, password);
            request.onResponse = (IHttpRequest myRequest, IHttpResponse response) =>
            {
                bool status = false;
                string token = string.Empty;
                if (string.IsNullOrEmpty(response.Error))
                {
                    Dictionary<string, object> dict = JsonUtil.Deserialize(response.Data);
                    if (dict.ContainsKey(Fields.SUCCESS))
                        status = (bool)dict[Fields.SUCCESS];
                    if (dict.ContainsKey(Fields.TOKEN))
                        token = dict[Fields.TOKEN].ToString();
                }
                onGetTokenCallback(response, status, token);
            };
            PuMain.WWWHandler.Request(request);
        }
    }
}
