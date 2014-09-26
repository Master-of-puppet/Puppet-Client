using Puppet.Utils;
using System;
using System.Collections.Generic;

namespace Puppet.Core.Network.Http
{
    public sealed class HttpPool
    {
        public static void GetAppConfig()
        {
            Request(Commands.GET_APPLICATION_CONFIG, GetVersion(), null);
        }

        public static void CheckVersion()
        {
            Request(Commands.CHECK_VERSION, GetVersion(), (bool status, string message) =>
            {
                if (status)
                {
                    Dictionary<string, object> currentDict = JsonUtil.Deserialize(message);
                    int code = (int)currentDict["code"];
                    EUpgrade type = (EUpgrade)code;
                    string url = currentDict.ContainsKey("market") ? currentDict["market"].ToString() : string.Empty;
                    PuMain.Instance.Dispatcher.SetWarningUpgrade(type, currentDict["message"].ToString(), url);
                }
            });
        }

        public static void ChangeUseInformation(string username, string password, string newpass)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("username", username);
            dict.Add("password", password);
            dict.Add("newPassword", newpass);
            dict.Add("renewPassword", newpass);

            Request(Commands.CHANGE_USER_INFORMATION, dict, null, "type", "changePassword");
        }

        public static void QuickRegister(string username, string password, DelegateAPICallback callback)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("username", username);
            dict.Add("password", password);

            Request(Commands.QUICK_REGISTER, dict, (bool status, string data) => HandleCallback(status, data, ref callback));
        }

        static Dictionary<string, string> GetVersion()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("code_application", "foxpocker");
            dict.Add("code_platform", "web");
            dict.Add("major", "1");
            dict.Add("minor", "0");
            dict.Add("patch", "0");
            dict.Add("build", "100");
            dict.Add("distributor", "foxpocker");
            return dict;
        }

        static void HandleCallback(bool status, string data, ref DelegateAPICallback callback)
        {
            bool responseStatus = false;
            string message = string.Empty;
            
            if (status)
            {
                Dictionary<string, object> currentDict = JsonUtil.Deserialize(data);
                int code = int.Parse(currentDict["code"].ToString());
                responseStatus = code == 0;
                message = currentDict["message"].ToString();
            }
            else
                message = data;

            if (callback != null)
                callback(responseStatus, message);
        }

        static void Request(string command, Dictionary<string, string> dict, Action<bool, string> callback, params object[] postData)
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
                if (callback != null)
                    callback(status, status ? response.Data : response.Error);
            };
            PuMain.WWWHandler.Request(request);
        }
    }
}
