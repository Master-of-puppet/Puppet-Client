using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Model;

namespace Puppet.Core.Network.Socket
{
    public class RequestPool
    {
        public static ISocketRequest GetLoginRequest(string token)
        {
            ISFSObject obj = new SFSObject();
            obj.PutUtfString(Fields.TOKEN, token);
            obj.PutSFSObject(Fields.CLIENT_DETAILS, new DataClientDetails().ToSFSObject());
            return new SFSocketRequest(new LoginRequest(string.Empty, string.Empty, PuMain.Setting.ZoneName, obj));
        }

        public static ISocketRequest GetJoinRoomRequest(int roomId)
        {
            ISFSObject obj = new SFSObject();
            obj.PutInt("roomId", roomId);
            return new SFSocketRequest(new ExtensionRequest("joinRoomRequest", obj));
        }
    }
}
