using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.API
{
    public class APIAuthentication
    {
        public void Login(string username, string password, Action<bool, string> onLoginCallback)
        {
            onLoginCallback(false, "Thông tin đăng nhập không hợp lệ.");
        }

        public void SocialLogin(string socialType, string accessToken, Action<bool, string> onLoginCallback)
        {
            onLoginCallback(false, "AccessToken đã hết hạn.");
        }

        public void Register(Dictionary<string, string> registerInformation, Action<bool, string> onRegisterCallback)
        {
            onRegisterCallback(false, "Địa chỉ email đã tồn tại trong hệ thống.");
        }
    }
}
