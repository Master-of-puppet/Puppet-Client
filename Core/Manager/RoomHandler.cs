using Puppet.Core.Network.Socket;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Utils;
using Puppet.Core.Model;
using Puppet.Core.Model.Factory;

namespace Puppet.Core
{
    internal class RoomHandler : BaseSingleton<RoomHandler>
    {
        Room _sfsCurrentRoom;

        protected override void Init() {}

        internal Room Current
        {
            get { return _sfsCurrentRoom; }
        }
        internal void SetCurrentRoom(ISocketResponse response)
        {
            if (response.Params.Contains("room"))
            {
                _sfsCurrentRoom = (Room)response.Params["room"];
                Logger.Log("Changed room!");

                //For debug show log RoomVariable
                foreach (RoomVariable r in _sfsCurrentRoom.GetVariables())
                    Logger.Log("RoomVariable: {0} - {1}", r.Name, ((SFSObject)r.Value).GetDump());
            }
        }

        internal string GetSceneNameFromCurrentRoom
        {
            get
            {
                SFSObject obj = GetValueRoomVariable("info");
                return obj != null && obj.ContainsKey("scene") ? obj.GetUtfString("scene") : string.Empty;
            }
        }

        internal RoomInfo GetParentRoom
        {
            get
            {
                SFSObject obj = GetValueRoomVariable("parent");
                return obj == null ? null : SFSDataModelFactory.CreateDataModel<RoomInfo>(obj);
            }
        }

        internal void LoadGroupLobby(out List<DataChannel> GroupsLobby)
        {
            GroupsLobby = new List<DataChannel>();
            SFSObject obj = GetValueRoomVariable("groups");
            if (obj != null && obj.ContainsKey("content"))
            {
                ISFSArray array = obj.GetSFSArray("content");
                for (int i = 0; i < array.Size(); i++)
                    GroupsLobby.Add(SFSDataModelFactory.CreateDataModel<DataChannel>(array.GetSFSObject(i)));
            }
        }

        internal SFSObject GetValueInfoRoomVariable
        {
            get { return GetValueRoomVariable("info"); }
        }

        SFSObject GetValueRoomVariable(string field)
        {
            if (_sfsCurrentRoom.ContainsVariable(field))
            {
                RoomVariable roomVar = _sfsCurrentRoom.GetVariable(field);
                return (SFSObject)roomVar.Value;
            }
            return null;
        }
        
    }
}
