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
        UserInfo _self;

        protected override void Init(){}

        internal void SetCurrentUser(ISocketResponse response)
        {
            if (response.Params.Contains("user"))
            {
                User user = (User)response.Params["user"];
                if (_mySelf == null || (_mySelf != null && _mySelf.Id == user.Id))
                {
                    _mySelf = user;

                    SFSObject infoObj = GetValueUserVariable("info");
                    SFSObject assetObj = GetValueUserVariable("assets");
                    DataUser dataUser = SFSDataModelFactory.CreateDataModel<DataUser>(infoObj);
                    DataAssets dataAsset = SFSDataModelFactory.CreateDataModel<DataAssets>(assetObj);
                    if (_self == null) 
                        _self = new UserInfo(dataUser, dataAsset);
                    else 
                        _self.UpdateInfo(dataUser, dataAsset);

                    Logger.Log(ELogColor.GREEN, "Updated User Infomations!!!");
                }

                //For debug show log RoomVariable
                foreach (UserVariable r in _mySelf.GetVariables())
                    Logger.Log("UserVariable: [" + r.Name + "]", ((SFSObject)r.Value).GetDump(), ELogColor.CYAN);
            }
        }


        internal UserInfo Self
        {
            get { return _self; }
        }

        internal void UpdateAsset(DataAssetItem[] newContent)
        {
            _self.assets.UpdateAssets(newContent);
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
