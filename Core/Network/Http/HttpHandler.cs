using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Network.Http
{
    public class HttpHandler : IHttp
    {
        IHttp _http;
        EEngine engine;

        public HttpHandler(IServerMode server, EEngine _engine)
        {
            this.engine = _engine;
            switch(engine)
            {
                case EEngine.Unity:
                    _http = new UnityHttp(server);
                    break;
                default :
                    NotSupport();
                    break;
            }
        }

        public void Request(IHttpRequest request)
        {
            switch (engine)
            {
                case EEngine.Unity:
                    _http.Request(request);
                    break;
                default:
                    NotSupport();
                    break;
            }
        }

        void NotSupport()
        {
            Logger.LogWarning("Not yet support other platforms, please implement IHttp for {0} Engine", engine);
        }
    }
}
