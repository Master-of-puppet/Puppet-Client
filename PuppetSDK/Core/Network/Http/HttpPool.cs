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

        #region CHANGE USER INFORMATION
        /// <summary>
        /// Change password
        /// </summary>
        internal static void ChangeUseInformation(string username, string password, string newpass, DelegateAPICallback callback)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("username", username);
            dict.Add("password", password);
            dict.Add("newPassword", newpass);
            dict.Add("renewPassword", newpass);

            Request(Commands.CHANGE_USER_INFORMATION, dict, (bool status, string data) => HandleCallback(status, data, ref callback), "type", "changePassword");
        }

        /// <summary>
        /// Change Avatar
        /// </summary>
        internal static void ChangeUseInformation(string username, byte[] avatar, DelegateAPICallback callback)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("username", username);
            if(avatar != null)
                dict.Add("avatar", StringUtil.ConvertToBinary(avatar));

            Request(Commands.CHANGE_USER_INFORMATION, dict, (bool status, string data) => HandleCallback(status, data, ref callback), "type", "changeAvatar");
        }

        /// <summary>
        /// Change infomation
        /// </summary>
        /// <param name="gender">Male is 0; Female is 1; Unknows is 2</param>
        /// <param name="address"></param>
        /// <param name="birthday">dd/mm/yyyy</param>
        internal static void ChangeUseInformation(string username, string firstName, string lastName, string middleName, string gender, string address, string birthday, DelegateAPICallback callback)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("username", username);
            dict.Add("first_name", firstName);
            dict.Add("last_name", lastName);
            dict.Add("middle_name", middleName);
            dict.Add("gender", gender);
            dict.Add("address", address);
            dict.Add("birthday", birthday);

            Request(Commands.CHANGE_USER_INFORMATION, dict, (bool status, string data) => HandleCallback(status, data, ref callback), "type", "changeInformation");
        }

        /// <summary>
        /// Change Information Special
        /// </summary>
        internal static void ChangeUseInformationSpecial(string username, string email, string mobile, DelegateAPICallback callback)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("username", username);
            dict.Add("email", email);
            dict.Add("mobile", mobile);

            Request(Commands.CHANGE_USER_INFORMATION, dict, (bool status, string data) => HandleCallback(status, data, ref callback), "type", "changeInformationSpecial");
        }
        #endregion

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

        internal static void GetInfoRecharge(Action<bool, string, DataResponseRecharge> callback)
        {
            Request(Commands.GET_INFO_RECHARGE, GetVersion(), (bool status, string jsonData) => 
            {
                if (status)
                {
                    DataResponseRecharge data = Puppet.Core.Model.Factory.JsonDataModelFactory.CreateDataModel<DataResponseRecharge>(jsonData);

                    bool responseStatus = false;
                    string responseMessage = string.Empty;
                    HandleCallback(status, jsonData, out responseStatus, out responseMessage);

                    if (callback != null && data != null)
                        callback (responseStatus, responseMessage, data);
                }
                else if (callback != null)
                {
                    callback (false, jsonData, null);
                }
            });
        }

        internal static void RechargeCard(string userName, string pin, string serial, string type, DelegateAPICallback callback)
        {
            Dictionary<string, string> dict = new Dictionary<string,string>();
            dict.Add("username", userName);
            dict.Add("pin", pin);
            dict.Add("serial", serial);
            dict.Add("type", type);
            Request(Commands.RECHARGE_CARD, dict, (bool status, string message) => HandleCallback(status, message, ref callback));
        }

        internal static void PostFacebook(string userName, string accessToken, string title, byte[] picture, DelegateAPICallback callback)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("username", userName);
            dict.Add("accessToken", accessToken);
            dict.Add("title", title);
            if (picture != null)
                dict.Add("picture", StringUtil.ConvertToBinary(picture));

            Request(Commands.POST_FACEBOOK, dict, (bool status, string message) => HandleCallback(status, message, ref callback));
        }

        internal static void SaveRequestFB(string facebookId, string[] requestIds, DelegateAPICallback callback)
        {
            string request = string.Empty;
            if (requestIds.Length > 0)
            {
                request = requestIds[0];
                for (int i = 1; i < requestIds.Length; i++)
                    request += string.Format(",{0}", requestIds[i]);
            }

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("fb_id", facebookId);
            dict.Add("request_ids", request);

            Request(Commands.SAVE_REQUEST_FB, dict, (bool status, string message) => HandleCallback(status, message, ref callback));
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
            HandleCallback(status, data, out responseStatus, out message);

            if (callback != null)
                callback(responseStatus, message);
        }

        static void HandleCallback(bool status, string responseData, out bool responseStatus, out string message)
        {
            responseStatus = false;
            message = string.Empty;

            if (status)
            {
                Dictionary<string, object> currentDict = JsonUtil.Deserialize(responseData);
                int code = int.Parse(currentDict["code"].ToString());
                responseStatus = code == 0;
                message = currentDict["message"].ToString();
            }
            else
                message = responseData;
        }

        static void Request(string command, Dictionary<string, string> jsonData, Action<bool, string> callback, params object[] postData)
        {
            List<object> lst = new List<object>();
            if (postData != null)
                lst = new List<object>(postData);
            lst.Add("data");
            lst.Add(JsonUtil.Serialize(jsonData));

            SimpleHttpRequest request = new SimpleHttpRequest(command, lst.ToArray());
            request.onResponse = (IHttpRequest myRequest, IHttpResponse response) =>
            {
                bool status = string.IsNullOrEmpty(response.Error);
                //Logger.Log(status ? response.Data : response.Error);
                if (callback != null)
                    PuMain.Setting.Threading.QueueOnMainThread(() => callback(status, status ? response.Data : response.Error));
            };
            PuMain.WWWHandler.Request(request);
        }
    }
}
