using Sfs2X.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Network.Socket;

namespace Puppet.Core.Network.Socket
{
    public class SFSocketRequest : ISocketRequest
    {
        IRequest _request;
        public SFSocketRequest(IRequest r)
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
