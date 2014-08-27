using Puppet.Core.Network.Http;
using System;
using System.Collections.Generic;
using Puppet.Utils;

namespace Puppet.API
{
    internal class APIAuthentication
    {
        internal static void GetAccessToken(string userName, string password, Action<IHttpResponse, bool, string> onGetTokenCallback)
        {
            SimpleHttpRequest request = new SimpleHttpRequest(Commands.GET_ACCESS_TOKEN, Fields.USERNAME, userName, Fields.PASSWORD, password);
            request.onResponse = (IHttpRequest myRequest, IHttpResponse response) =>
            {
                bool status = false;
                string token = string.Empty;
                if (string.IsNullOrEmpty(response.Error))
                {
                    Dictionary<string, object> dict = JsonUtil.Deserialize(response.Data);
                    if (dict.ContainsKey(Fields.SUCCESS))
                        status = (bool)dict[Fields.SUCCESS];
                    if (dict.ContainsKey(Fields.TOKEN))
                        token = dict[Fields.TOKEN].ToString();
                }
                onGetTokenCallback(response, status, token);
            };
            PuMain.WWWHandler.Request(request);
        }
    }
}
