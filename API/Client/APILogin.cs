using Puppet.Core.Flow;
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
        /// <summary>
        /// Yêu cầu đăng nhập hệ thống
        /// </summary>
        /// <param name="token">Với accessToken đã có</param>
        /// <param name="onLoginCallback">Callback khi kết thúc đăng nhập</param>
        public static void Login(string token, Action<bool, string> onLoginCallback)
        {
            if(SceneHandler.Instance.Current.SceneType != EScene.LoginScreen)
            {
                onLoginCallback(false, "API chỉ được thực thi khi ở màn Login");
                return;
            }

            SceneLogin.Instance.Login(token, onLoginCallback);
        }

        public static void SocialLogin(string socialType, string accessToken, Action<bool, string> onLoginCallback)
        {
            if (SceneHandler.Instance.Current.SceneType != EScene.LoginScreen)
            {
                onLoginCallback(false, "API chỉ được thực thi khi ở màn Login");
                return;
            }

            onLoginCallback(false, "AccessToken đã hết hạn.");
        }

        public static void Register(Dictionary<string, string> registerInformation, Action<bool, string> onRegisterCallback)
        {
            if (SceneHandler.Instance.Current.SceneType != EScene.LoginScreen)
            {
                onRegisterCallback(false, "API chỉ được thực thi khi ở màn Login");
                return;
            }

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
            if (SceneHandler.Instance.Current.SceneType != EScene.LoginScreen)
            {
                onGetTokenCallback(null, false, "API chỉ được thực thi khi ở màn Login");
                return;
            }

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
