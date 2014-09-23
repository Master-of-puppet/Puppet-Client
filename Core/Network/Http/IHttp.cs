using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Puppet.Core.Network.Http
{
    public interface IHttp
    {
        void Request(IHttpRequest request);
        void Request(IHttpRequest request, IServerMode serverMode);
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
        HttpStatusCode State { get; set; }
        string Error { get; set; }
        string Data { get; set; }
    }

    public enum HttpMethod
    {
        Get,
        Post,
    }
}
