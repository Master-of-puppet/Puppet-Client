using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puppet.Core.Network.Http
{
    sealed class BaseHttp : IHttp
    {
        IServerMode serverMode;
        public BaseHttp(IServerMode server)
        {
            this.serverMode = server;
        }

        public void Request(IHttpRequest request)
        {
            request.Start(serverMode);
        }

        public void Request(IHttpRequest request, IServerMode serverMode)
        {
            request.Start(serverMode);
        }
    }
}
