﻿using Puppet.Core.Flow;
using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet
{
    internal class PuGlobal : BaseSingleton<PuGlobal>
    {
        internal RoomInfo FirtRoomToJoin;
        internal DataGame SelectedGame;
        internal DataChannel SelectedChannel;
        internal List<DataChannel> GroupsLobby;
        internal DataLobby SelectedLobby;
        internal DataDailyGift CurrentDailyGift;

        protected override void Init()
        {
        }
    }
}
