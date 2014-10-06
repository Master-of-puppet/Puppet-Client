using Puppet.Core.Network.Socket;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using Puppet.Core.Model.Datagram;

namespace Puppet.Poker.Datagram
{
    internal static class RequestPool
    {
        public static ISocketRequest GetSitRequest(PokerRequestPlay request, int slotIndex)
        {
            ISFSObject obj = new RequestPlugin(new RequestGameAction(request.ToString().ToLower(), slotIndex), RequestPlugin.GAME_PLUGIN_VALUE).ToSFSObject();
            return new SFSocketRequest(new ExtensionRequest(Fields.REQUEST_PLUGIN, obj, PuMain.Instance.SfsRoom));
        }

        public static ISocketRequest GetPlayRequest(PokerRequestPlay request, long value)
        {
            ISFSObject obj = new RequestPlugin(new RequestGameAction(request.ToString().ToLower(), value), RequestPlugin.GAME_PLUGIN_VALUE).ToSFSObject();
            return new SFSocketRequest(new ExtensionRequest(Fields.REQUEST_PLUGIN, obj, PuMain.Instance.SfsRoom));
        }
    }
}
