using System;
using System.Collections.Generic;
using Puppet.Utils;
using Puppet.Core.Model;
using Puppet.Core.Network.Http;

namespace Puppet.Core.Manager
{
    internal class LoadConfig
    {
        internal LoadConfig()
        {
            GetAppConfig();
            ChangeUseInformation();
        }


        void GetAppConfig()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("code_application", "foxpocker");
            dict.Add("code_platform", "web");
            dict.Add("major", "1");
            dict.Add("minor", "0");
            dict.Add("patch", "0");
            dict.Add("build", "100");
            dict.Add("distributor", "foxpocker");

            Request(Commands.GET_APPLICATION_CONFIG, dict, null);
        }

        void ChangeUseInformation()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("username", "thanhnd");
            dict.Add("password", "thanhnd");
            dict.Add("newPassword", "cnttbkhn");
            dict.Add("renewPassword", "cnttbkhn89");

            Request(Commands.CHANGE_USER_INFORMATION, dict, null, "type", "changePassword");
        }

        void Request(string command, Dictionary<string, string> dict, Action<bool, string> callback, params object[] postData)
        {
            List<object> lst = new List<object>();
            if (postData != null)
                lst = new List<object>(postData);
            lst.Add("data");
            lst.Add(JsonUtil.Serialize(dict));

            SimpleHttpRequest request = new SimpleHttpRequest(command, lst.ToArray());
            request.onResponse = (IHttpRequest myRequest, IHttpResponse response) =>
            {
                bool status = string.IsNullOrEmpty(response.Error);
                Logger.Log(status ? response.Data : response.Error);
                if(callback != null)
                    callback(status, status ? response.Data : response.Error);
            };
            PuMain.WWWHandler.Request(request);
        }
    }
}
