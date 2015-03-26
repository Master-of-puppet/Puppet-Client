using Puppet.Core.Model;
using Puppet.Core.Modules.Buddy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.API.Client
{
    public sealed class APIFriend
    {
        /// <summary>
        /// Thêm người chơi vào danh sách bạn bè
        /// </summary>
        public static void AddBuddy(DataUser user)
        {
            BuddyHandler.Instance.AddBuddy(new DataBuddy(user.id, user.userName));
        }

        /// <summary>
        /// Xóa bỏ người chơi khởi danh sách bạn bè
        /// </summary>
        public static void RemoveBuddy(DataUser user)
        {
            BuddyHandler.Instance.RemoveBuddy(new DataBuddy(user.id, user.userName));
        }
    }
}
