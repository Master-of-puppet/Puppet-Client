using Puppet.Core.Network.Http;
using Puppet.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.API.Client
{
    public sealed class APILogin
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
            APIAuthentication.GetAccessToken(userName, password, onGetTokenCallback);
        }
    }
}
