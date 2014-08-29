using Sfs2X.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Network.Socket;

namespace Puppet.Core.Network.Socket
{
    internal class SFSocketRequest : ISocketRequest
    {
        IRequest _request;
        internal SFSocketRequest(IRequest r)
        {
            this._request = r;
        }

        public IRequest Resquest
        {
            get
            {
                return _request;
            }
        }
    }
}
