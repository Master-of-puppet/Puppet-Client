using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Network.Http
{
    public interface IHttp
    {
        void Request(IHttpRequest request);
    }

    public interface IHttpRequest
    {
        void Start(IServerMode server);
        float TimeOut { get; set; }
        HttpMethod Method { get; set; }
        Action<IHttpRequest, float> onProgress { get; set; }
        Action<IHttpRequest, IHttpResponse> onResponse { get; set; }
    }

    public interface IHttpResponse
    {
        StateResponse State { get; set; }
        string Error { get; set; }
        string Data { get; set; }
    }

    public enum StateResponse
    {
        None,
        Request_Timeout,
		Network_Fail,
		Missing_Parameter,
		Invalid_Input,
		Invalid_Json,
		Success, //200
		Server_Error, //500
		Server_Maintenance,//503
		Unsupport_App, //505
		Unauthorized, // 401,
		Missing_Request_Header, //417
    }

    public enum HttpMethod
    {
        Get,
        Post,
    }
}
