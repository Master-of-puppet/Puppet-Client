using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Model;
using Puppet.Core.Model.Factory;
using Puppet.Core.Network.Socket;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;

namespace Puppet.Core
{
    internal class UserHandler : BaseSingleton<UserHandler>
    {
        User _mySelf;

        protected override void Init(){}

        internal void SetCurrentUser(ISocketResponse response)
        {
            if (response.Params.Contains("user"))
            {
                _mySelf = (User)response.Params["user"];
                Logger.Log("Updated User Infomations!!!");

                //For debug show log RoomVariable
                foreach (UserVariable r in _mySelf.GetVariables())
                    Logger.Log("{0} - {1}", r.Name, ((SFSObject)r.Value).GetDump());
            }
        }

        internal UserInfo Self
        {
            get
            {
                SFSObject infoObj = GetValueUserVariable("info");
                SFSObject assetObj = GetValueUserVariable("assets");
                DataUser user = SFSDataModelFactory.CreateDataModel<DataUser>(infoObj);
                DataAssets asset = SFSDataModelFactory.CreateDataModel<DataAssets>(assetObj);
                return new UserInfo(user, asset);
            }
        }

        SFSObject GetValueUserVariable(string field)
        {
            if (_mySelf.ContainsVariable(field))
            {
                UserVariable roomVar = _mySelf.GetVariable(field);
                return (SFSObject)roomVar.Value;
            }
            return null;
        }
    }
}
