using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Model;
using Puppet.Core.Model.Datagram;
using Puppet.Utils;

namespace Puppet.Core.Network.Socket
{
    public class RequestPool
    {
        public static ISocketRequest GetLoginRequest(string token)
        {
            ISFSObject obj = new RequestLogin(token).ToSFSObject();
            return new SFSocketRequest(new LoginRequest(string.Empty, string.Empty, string.Empty, obj));
        }

        public static ISocketRequest GetJoinRoomRequest(RoomInfo room)
        {
            ISFSObject obj = room.ToSFSObject();
            return new SFSocketRequest(new ExtensionRequest(Fields.REQUEST_JOIN_ROOM, obj));
        }

        public static ISocketRequest GetRequestNodePluginWithCommand(string command)
        {
            ISFSObject obj = new RequestNodePlugin(new RequestCommand(command)).ToSFSObject();
            return new SFSocketRequest(new ExtensionRequest(Fields.REQUEST_PLUGIN, obj, RoomHandler.Instance.Current));
        }

        public static ISocketRequest GetRequestGetChidren()
        {
            return GetRequestNodePluginWithCommand(Fields.COMMAND_GET_CHIDREN);
        }
    }
}
