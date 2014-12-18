using Puppet.Core.Model;
using Puppet.Utils;
using System;
using System.Collections.Generic;

namespace Puppet.Core.Network.Http
{
    internal sealed class HttpPool
    {
        internal static void GetAppConfig()
        {
            Request(Commands.GET_APPLICATION_CONFIG, GetVersion(), null);
        }

        internal static void CheckVersion()
        {
            Request(Commands.CHECK_VERSION, GetVersion(), (bool status, string message) =>
            {
                if (status)
                {
                    Dictionary<string, object> currentDict = JsonUtil.Deserialize(message);
                    int code = int.Parse(currentDict["code"].ToString());
                    EUpgrade type = (EUpgrade)code;
                    string url = currentDict.ContainsKey("market") ? currentDict["market"].ToString() : string.Empty;
                    PuMain.Dispatcher.SetWarningUpgrade(type, currentDict["message"].ToString(), url);
                }
            });
        }

        internal static void RequestChangePassword(string yourEmail, DelegateAPICallback callback)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Request(Commands.FORGOT_PASSWORD, dict, (bool status, string data) => HandleCallback(status, data, ref callback), "email", yourEmail);
        }

        internal static void ChangeUseInformation(string username, string password, string newpass, DelegateAPICallback callback)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("username", username);
            dict.Add("password", password);
            dict.Add("newPassword", newpass);
            dict.Add("renewPassword", newpass);

            Request(Commands.CHANGE_USER_INFORMATION, dict, (bool status, string data) => HandleCallback(status, data, ref callback), "type", "changePassword");
        }

        internal static void QuickRegister(string username, string password, DelegateAPICallback callback)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Request(Commands.QUICK_REGISTER, dict, (bool status, string data) => HandleCallback(status, data, ref callback), "username", username, "password", password);
        }

        internal static void GetAccessTokenFacebook(string facebookToken, DelegateAPICallbackDictionary callback)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Dictionary<string, object> responseDict = new Dictionary<string, object>();

            Request(Commands.GET_ACCESS_TOKEN, dict, (bool status, string data) => 
            {
                bool responseStatus = false;
                string message = string.Empty;
                if (status)
                {
                    responseDict = JsonUtil.Deserialize(data);
                    int code = int.Parse(responseDict["code"].ToString());
                    responseStatus = code == 0;
                    message = responseDict["message"].ToString();
                }
                else
                    message = data;

                if (callback != null)
                    callback(responseStatus, message, responseDict);
            }, "type", "facebook", "accessToken", facebookToken);
        }

        internal static void RegisterWithFacebook(string username, string password, DelegateAPICallback callback)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Request(Commands.GET_ACCESS_TOKEN, dict, (bool status, string data) => HandleCallback(status, data, ref callback), "username", username, "password", password, "type", "register");
        }

        internal static void GetInfoRecharge(Action<DataResponseRecharge> callback)
        {
            Request(Commands.GET_INFO_RECHARGE, GetVersion(), (bool status, string jsonData) => 
            {
                if (status)
                {
                    DataResponseRecharge data = Puppet.Core.Model.Factory.JsonDataModelFactory.CreateDataModel<DataResponseRecharge>(jsonData);
                    if (callback != null && data != null)
                        callback(data);
                }
            });
        }

        static Dictionary<string, string> GetVersion()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("code_application", "foxpoker");
            dict.Add("code_platform", "web-html5");
            dict.Add("major", "1");
            dict.Add("minor", "0");
            dict.Add("patch", "0");
            dict.Add("build", "100");
            dict.Add("distributor", "foxpoker");
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
                //Logger.Log(status ? response.Data : response.Error);
                if (callback != null)
                    callback(status, status ? response.Data : response.Error);
            };
            PuMain.WWWHandler.Request(request);
        }
    }
}
