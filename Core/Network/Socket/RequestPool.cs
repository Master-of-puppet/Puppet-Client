using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Model;
using Puppet.Core.Model.Datagram;
using Puppet.Utils;
using Puppet.Core.Flow;

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

        public static ISocketRequest GetRequestNodePlugin(AbstractData data)
        {
            ISFSObject obj = new RequestNodePlugin(data).ToSFSObject();
            return new SFSocketRequest(new ExtensionRequest(Fields.REQUEST_PLUGIN, obj, RoomHandler.Instance.Current));
        }

        public static ISocketRequest GetRequestGetChidren()
        {
            return GetRequestNodePlugin(new RequestCommand(Fields.COMMAND_GET_CHIDREN));
        }

        public static ISocketRequest GetRequestGetGroupChildren(string groupName)
        {
            return GetRequestNodePlugin(new RequestGetGroups(Fields.COMMAND_GET_GROUP_CHILDREN, groupName));
        }

        public static ISocketRequest GetRequestCreateLobby()
        {
            return GetRequestNodePlugin(new RequestGetGroups(Fields.COMMAND_CREATE_GAME, ScenePockerLobby.Instance.selectedChannel.name, SceneWorldGame.Instance.selectedGame.roomName));
        }
    }
}
