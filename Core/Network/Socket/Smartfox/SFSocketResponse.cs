using Sfs2X.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Network.Socket
{
    internal class SFSocketResponse : ISocketResponse
    {
        BaseEvent response;

        internal SFSocketResponse(BaseEvent e)
        {
            this.response = e;
        }

        public System.Collections.IDictionary Params
        {
            get
            {
                return response.Params;
            }
        }

        public string Type
        {
            get
            {
                return response.Type;
            }
            
        }
    }
}
