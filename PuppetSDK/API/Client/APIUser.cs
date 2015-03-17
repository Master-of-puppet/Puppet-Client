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
        private static string USERNAME
        {
            get { return PuGlobal.Instance.mUserInfo.info.userName; }
        }

        #region Change Use Information
        /// <summary>
        /// Thay đổi mật khẩu người chơi
        /// </summary>
        /// <param name="password">Mật khẩu hiện tại</param>
        /// <param name="newpass">Mật khẩu mới</param>
        /// <param name="callback">Hành động khi hoàn tất đổi thông tin</param>
        public static void ChangeUseInformation(string password, string newpass, DelegateAPICallback callback)
        {
            if (string.IsNullOrEmpty(password))
            {
                if (callback != null) callback(false, "ERROR: Mật khẩu hiện tại chưa được nhập.");
                return;
            }
            else if (string.IsNullOrEmpty(newpass))
            {
                if (callback != null) callback(false, "ERROR: Vui lòng nhập mật khẩu mới.");
                return;
            }

            HttpPool.ChangeUseInformation(USERNAME, password, newpass, callback);
        }

        /// <summary>
        /// Thay đổi avatar của người chơi
        /// </summary>
        /// <param name="avatar">Thông tin về ảnh (byte[]) mới</param>
        /// <param name="callback">Hành động khi hoàn tất đổi thông tin</param>
        /// 
        public static void ChangeUseInformation(byte[] avatar, DelegateAPICallback callback)
        {
            if (avatar == null)
            {
                if (callback != null) callback(false, "ERROR: Không có dữ liệu về avatar mới.");
                return;
            }

            HttpPool.ChangeUseInformation(USERNAME, avatar, callback);
        }
       
        /// <summary>
        /// Thay đổi thông tin cá nhân của người chơi
        /// </summary>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        /// <param name="middleName">Middle Name</param>
        /// <param name="gender">Giới tính: Male is 0; Female is 1; Unknow is 2</param>
        /// <param name="address">Địa chỉ</param>
        /// <param name="birthday">Ngày sinh (dd/MM/yyyy)</param>
        /// <param name="callback">Hành động khi hoàn tất đổi thông tin</param>
        public static void ChangeUseInformation(string firstName, string lastName, string middleName, int gender, string address, string birthday, DelegateAPICallback callback)
        {
            DateTime date;
            if (DateTime.TryParseExact(birthday, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
            {
                if (callback != null) callback(false, "ERROR: Định dạng sinh không chính xác. Vui lòng sử dụng định dạng dd/MM/yyyy.");
                return;
            }

            if(gender >= 0 && gender <= 2)
            {
                HttpPool.ChangeUseInformation(USERNAME, firstName, lastName, middleName, gender.ToString(), address, birthday, callback);
            }
            else if(callback != null )
            {
                callback(false, "ERROR: Thông tin về giới tính không hợp lệ. Vui lòng chọn 0 là Name, 1 là nữ, 2 là không xác định.");
            }
        }

        /// <summary>
        /// Thay đổi mật khẩu người chơi
        /// </summary>
        /// <param name="email">Emial mới</param>
        /// <param name="mobile">Mobile mới</param>
        /// <param name="callback">Hành động khi hoàn tất đổi thông tin</param>
        public static void ChangeUseInformationSpecial(string email, string mobile, DelegateAPICallback callback)
        {
            HttpPool.ChangeUseInformationSpecial(USERNAME, email, mobile, callback);
        }
        #endregion

        /// <summary>
        /// Lấy về các thông tin về người chơi, Như tài sản, thông tin cá nhân...
        /// </summary>
        public static UserInfo GetUserInformation()
        {
            return UserHandler.Instance.Self;
        }

        /// <summary>
        /// Không được gọi hàm này khi không hiểu nhé. Tạm thời chỉ sử dụng để các API gọi ngược vào Core SDK thôi.
        /// </summary>
        public static void UpdateAssetInfo(DataAssetItem[] newContents)
        {
            UserHandler.Instance.UpdateAsset(newContents);
        }
    }
}
