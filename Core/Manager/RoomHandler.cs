using Puppet.Core.Network.Socket;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core
{
    internal class RoomHandler : BaseSingleton<RoomHandler>
    {
        Room _sfsCurrentRoom;

        protected override void Init() {}

        public Room Current
        {
            get { return _sfsCurrentRoom; }
        }
        public void SetCurrentRoom(ISocketResponse response)
        {
            if (response.Params.Contains("room"))
            {
                _sfsCurrentRoom = (Room)response.Params["room"];
                Logger.Log("Changed room!");
            }
        }

        public string GetSceneNameFromCurrentRoom
        {
            get
            {
                if (_sfsCurrentRoom.ContainsVariable("info"))
                {
                    RoomVariable roomVar = _sfsCurrentRoom.GetVariable("info");
                    SFSObject obj = (SFSObject)roomVar.Value;
                    if (obj.ContainsKey("scene"))
                        return obj.GetUtfString("scene");
                }
                return string.Empty;
            }
        }
        
    }
}
