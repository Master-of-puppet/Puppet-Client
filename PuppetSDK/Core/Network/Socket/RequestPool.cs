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
    internal sealed class RequestPool
    {
        internal static ISocketRequest GetLoginRequest(string token)
        {
            ISFSObject obj = new RequestLogin(token).ToSFSObject();
            return new SFSocketRequest(new LoginRequest(string.Empty, string.Empty, string.Empty, obj));
        }

        internal static ISocketRequest GetJoinRoomRequest(RoomInfo room)
        {
            ISFSObject obj = room.ToSFSObject();
            return new SFSocketRequest(new ExtensionRequest(Fields.REQUEST_JOIN_ROOM, obj));
        }

        internal static ISocketRequest GetQuickJoinRoomRequest()
        {
            return GetRequestPlugin(new RequestCommand(Fields.REQUEST_QUICK_JOIN_ROOM));
        }

        internal static ISocketRequest GetRequestPlugin(AbstractData data)
        {
            ISFSObject obj = new RequestPlugin(data).ToSFSObject();
            return new SFSocketRequest(new ExtensionRequest(Fields.REQUEST_PLUGIN, obj, RoomHandler.Instance.Current));
        }

        internal static ISocketRequest GetRequestGetChidren()
        {
            return GetRequestPlugin(new RequestCommand(Fields.COMMAND_GET_CHIDREN));
        }

        internal static ISocketRequest GetRequestGetGroupChildren(string groupName)
        {
            return GetRequestPlugin(new RequestGetGroups(Fields.COMMAND_GET_GROUP_CHILDREN, groupName));
        }

        internal static ISocketRequest GetRequestCreateLobby(double maxBet, int numberPlayer)
        {
            return GetRequestPlugin(new RequestCreateGame(Fields.COMMAND_CREATE_GAME, PuGlobal.Instance.SelectedChannel.name, PuGlobal.Instance.SelectedGame.roomName, new DataConfigGame(maxBet, numberPlayer)));
        }

        internal static ISocketRequest GetDailyGift(DataDailyGift data)
        {
            string pluginName = data.listenerPlugin ?? RequestPlugin.OBSERVER_PLUGIN_VALUE;
            ISFSObject obj = new RequestPlugin(new RequestGetGift(data), pluginName).ToSFSObject();
            return new SFSocketRequest(new ExtensionRequest(Fields.REQUEST_PLUGIN, obj));
        }

        internal static ISocketRequest GetLogout()
        {
            return new SFSocketRequest(new LogoutRequest());
        }

        internal static ISocketRequest GetPublicMessageRequest(string message, AbstractData data)
        {
            ISFSObject obj = SFSObject.NewInstance();
            if (data != null) obj = data.ToSFSObject();
            return new SFSocketRequest(new PublicMessageRequest(message, obj, RoomHandler.Instance.Current));
        }
    }
}
