using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Flow;
using Puppet.Core.Network.Http;
using Puppet.Core.Model;
using Puppet.Core;

namespace Puppet.API.Client
{
    public sealed class APIUser
    {
        /// <summary>
        /// Thay đổi thông tin cá nhân người chơi
        /// (Tạm chỉ đổi mật khẩu)
        /// </summary>
        /// <param name="username">Tên truy cập</param>
        /// <param name="password">Mật khẩu hiện tại</param>
        /// <param name="newpass">Mật khẩu mới</param>
        /// <param name="callback">Hành động khi hoàn tất đổi thông tin</param>
        public static void ChangeUseInformation(string username, string password, string newpass, DelegateAPICallback callback)
        {
            HttpPool.ChangeUseInformation(username, password, newpass, callback);
        }

        /// <summary>
        /// Lấy về các thông tin về người chơi, Như tài sản, thông tin cá nhân...
        /// </summary>
        public static UserInfo GetUserInformation()
        {
            return UserHandler.Instance.Self;
        }
    }
}
