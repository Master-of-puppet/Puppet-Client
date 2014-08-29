using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Network.Socket
{
    public class RequestPool
    {
        public static ISocketRequest GetLoginRequest(string token)
        {
            ISFSObject obj = new SFSObject();
            obj.PutUtfString(Fields.TOKEN, token);
            return new SFSocketRequest(new LoginRequest(string.Empty, string.Empty, PuMain.Setting.ZoneName, obj));
        }
    }
}
