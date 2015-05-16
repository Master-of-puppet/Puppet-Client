using Puppet.Core.Flow;
using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet
{
    internal class PuGlobal : BaseSingleton<PuGlobal>
    {
        internal string token;
        internal UserInfo mUserInfo;
        internal RoomInfo FirtRoomToJoin;
        internal DataGame SelectedGame;
        internal DataChannel SelectedChannel;
        internal List<DataChannel> GroupsLobby;
        internal RoomInfo SelectedRoomJoin;
        internal DataDailyGift CurrentDailyGift;
        internal List<string> listDefaultAvatar;

        protected override void Init()
        {
        }
    }
}
