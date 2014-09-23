using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Network.Http
{
    public class HttpHandler : IHttp
    {
        IHttp _http;

        internal HttpHandler(IServerMode server)
        {
            _http = new BaseHttp(server);
        }

        public void Request(IHttpRequest request)
        {
            _http.Request(request);
        }

        public void Request(IHttpRequest request, IServerMode serverMode)
        {
            IHttp http = new BaseHttp(serverMode);
            http.Request(request);
        }
    }
}
