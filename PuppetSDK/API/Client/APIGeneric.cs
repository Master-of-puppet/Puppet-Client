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
    public sealed class APIGeneric
    {
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
        public static void GetInfoRecharge(Action<DataResponseRecharge> callback)
        {
            HttpPool.GetInfoRecharge(callback);
        }
    }
}
