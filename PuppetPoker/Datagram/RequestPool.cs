using Puppet.Core.Network.Socket;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Puppet.Poker.Datagram
{
    internal static class RequestPool
    {
        public static ISocketRequest GetSitRequest(int slotIndex)
        {
            ISFSObject obj = SFSObject.NewInstance();
            obj.PutUtfString("action", "sit");
            obj.PutInt("slot", slotIndex);
            return new SFSocketRequest(new ExtensionRequest("play", obj, PuMain.Instance.SfsRoom));
        }

        public static ISocketRequest GetPlayRequest(RequestPlay request, long value)
        {
            ISFSObject obj = SFSObject.NewInstance();
            obj.PutUtfString("action", request.ToString().ToLower());
            obj.PutLong("value", value);
            return new SFSocketRequest(new ExtensionRequest("play", obj, PuMain.Instance.SfsRoom));
        }
    }
}
