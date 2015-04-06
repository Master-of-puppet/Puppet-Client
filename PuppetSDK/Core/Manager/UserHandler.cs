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
                User user = (User)response.Params["user"];
                if (_mySelf == null || (_mySelf != null && _mySelf.Id == user.Id))
                {
                    _mySelf = user;

                    SFSObject infoObj = GetValueUserVariable(_mySelf, "info");
                    SFSObject assetObj = GetValueUserVariable(_mySelf, "assets");
                    DataUser dataUser = SFSDataModelFactory.CreateDataModel<DataUser>(infoObj);
                    DataAssets dataAsset = SFSDataModelFactory.CreateDataModel<DataAssets>(assetObj);
                    if (Self == null)
                        Self = new UserInfo(dataUser, dataAsset);
                    else
                        Self.UpdateInfo(dataUser, dataAsset);

                    Logger.Log(ELogColor.GREEN, "Updated User Infomations!!!");
                }

                //For debug show log RoomVariable
                foreach (UserVariable r in _mySelf.GetVariables())
                    Logger.Log("UserVariable: [" + r.Name + "]", ((SFSObject)r.Value).GetDump(), ELogColor.CYAN);

                Logger.LogWarning("---> Data UserInfo Changed to: " + Self.info.ToString());

            }
        }


        internal UserInfo Self
        {
            get { return PuGlobal.Instance.mUserInfo; }
            private set { 
                PuGlobal.Instance.mUserInfo = value; 
            }
        }

        internal void UpdateAsset(DataAssetItem[] newContent)
        {
            Self.assets.UpdateAssets(newContent);
        }

        SFSObject GetValueUserVariable(User user, string field)
        {
            if (user.ContainsVariable(field))
            {
                UserVariable roomVar = user.GetVariable(field);
                return (SFSObject)roomVar.Value;
            }
            return null;
        }

        internal UserInfo ConvertUser(User user)
        {
            SFSObject infoObj = GetValueUserVariable(user, "info");
            SFSObject assetObj = GetValueUserVariable(user, "assets");
            DataUser dataUser = SFSDataModelFactory.CreateDataModel<DataUser>(infoObj);
            DataAssets dataAsset = SFSDataModelFactory.CreateDataModel<DataAssets>(assetObj);
            return new UserInfo(dataUser, dataAsset);
        }

    }
}
