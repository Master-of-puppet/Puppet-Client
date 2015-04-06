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
        public static void Login(string token, DelegateAPICallback onLoginCallback)
        {
            if(SceneHandler.Instance.Current.SceneType != EScene.LoginScreen)
            {
                onLoginCallback(false, "API chỉ được thực thi khi ở màn Login");
                return;
            }

            if(string.IsNullOrEmpty(token))
            {
                onLoginCallback(false, "Token không hợp lệ, vui lòng thử lại.");
                return;
            }

            SceneLogin.Instance.Login(token, onLoginCallback);
        }

        /// <summary>
        /// Yêu cầu đăng nhập kiểu chơi thử
        /// </summary>
        /// <param name="onLoginCallback">Callback khi xử lý xong đăng nhập</param>
        public static void LoginTrial(DelegateAPICallback onLoginCallback)
        {
            if (SceneHandler.Instance.Current.SceneType != EScene.LoginScreen)
            {
                onLoginCallback(false, "API chỉ được thực thi khi ở màn Login");
                return;
            }

            SceneLogin.Instance.Login(string.Empty, onLoginCallback);
        }

        /// <summary>
        /// API hỗ trợ việc đăng ký nhanh
        /// </summary>
        /// <param name="username">Tên truy cập</param>
        /// <param name="password">Mật khẩu</param>
        /// <param name="callback">Hành động trả về</param>
        public static void QuickRegister(string username, string password, DelegateAPICallback callback)
        {
            if (SceneHandler.Instance.Current.SceneType != EScene.LoginScreen)
            {
                callback(false, "API chỉ được thực thi khi ở màn Login");
                return;
            }
            
            HttpPool.QuickRegister(username, password, callback);
        }

        /// <summary>
        /// API yêu câu lấy accsessToken để đăng nhập cho hệ thống từ Facebook
        /// </summary>
        /// <param name="facebookToken">Facebook Token</param>
        /// <param name="callback">Hành động trả về</param>
        public static void GetAccessTokenFacebook(string facebookToken, DelegateAPICallbackDictionary callback)
        {
            if (SceneHandler.Instance.Current.SceneType != EScene.LoginScreen)
            {
                callback(false, "API chỉ được thực thi khi ở màn Login", null);
                return;
            }
            
            HttpPool.GetAccessTokenFacebook(facebookToken, callback);
        }

        /// <summary>
        /// API giúp đăng ký tài khoản vào hệ thống khi lần đầu đăng nhập bằng Facebook
        /// </summary>
        /// <param name="username">Tên truy cập</param>
        /// <param name="password">Mật khẩu</param>
        /// <param name="callback">Hàm thực thi khi có kết quả</param>
        public static void RegisterWithFacebook(string username, string password, DelegateAPICallback callback)
        {
            if (SceneHandler.Instance.Current.SceneType != EScene.LoginScreen)
            {
                callback(false, "API chỉ được thực thi khi ở màn Login");
                return;
            }

            HttpPool.RegisterWithFacebook(username, password, callback);
        }

        /// <summary>
        /// Lấy thông tin về AccessToken để đăng nhập vào hệ thống.
        /// </summary>
        /// <param name="userName">Tên truy cập</param>
        /// <param name="password">Mật khẩu</param>
        /// <param name="onGetTokenCallback">
        /// IHttpResponse: Phản hồi từ HTTP.
        /// bool: Kết quả thành công hay thất bại.
        /// string: Token lấy được nếu thành công.
        /// </param>
        public static void GetAccessToken(string userName, string password, DelegateAPICallbackHttpRequest onGetTokenCallback)
        {
            if (SceneHandler.Instance.Current.SceneType != EScene.LoginScreen)
            {
                onGetTokenCallback(false, "API chỉ được thực thi khi ở màn Login", null);
                return;
            }

            SimpleHttpRequest request = new SimpleHttpRequest(Commands.COMMAND_GET_ACCESS_TOKEN, Fields.USERNAME, userName, Fields.PASSWORD, password);
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
                    else if (dict.ContainsKey(Fields.DETAILS))
                        token = dict[Fields.DETAILS].ToString();

                    onGetTokenCallback(status, token, response);
                }
                else
                {
                    HttpPool.GetAccessToken(userName, password, (httpStatus, httpMessage, httpDict) =>
                    {
                        if (httpStatus && httpDict.ContainsKey("accessToken"))
                            token = httpDict["accessToken"].ToString();
                        
                        onGetTokenCallback(httpStatus, token, response);
                    });
                }
            };
            PuMain.WWWHandler.Request(request);
        }

        public static void RequestChangePassword(string yourEmail, DelegateAPICallback callback)
        {
            HttpPool.RequestChangePassword(yourEmail, callback);
        }
    }
}
