using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet
{
    public static class Commands
    {
        public const string COMMAND_GET_ACCESS_TOKEN    = "/realtime/puppet/?command=get_access_token";

        public const string GET_APPLICATION_CONFIG      = "/static/api/getApplicationConfig";
        public const string CHANGE_USER_INFORMATION     = "/static/api/ChangeUserInformation";
        public const string CHECK_VERSION               = "/static/api/checkVersion";
        public const string QUICK_REGISTER              = "/static/api/quickRegister";
        public const string GET_ACCESS_TOKEN            = "/static/api/getAccessToken";
        public const string GET_INFO_RECHARGE           = "/static/api/getInfoRecharge";
        public const string FORGOT_PASSWORD             = "/static/api/forgotPassword";
        public const string RECHARGE_CARD               = "/static/api/rechargeCard";
        public const string POST_FACEBOOK               = "/static/api/postFacebook";
        public const string SAVE_REQUEST_FB             = "/static/api/saveRequestFb";
        public const string GET_DEFAULT_AVATAR          = "/static/api/getDefaultAvatar";
        public const string SAVE_DEFAULT_AVATAR         = "/static/api/saveDefaultAvatar";

        public const string GET_EVENTS                  = "/api/getEvents";
    }
}
