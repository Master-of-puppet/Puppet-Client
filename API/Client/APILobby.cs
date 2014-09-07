using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Model;
using Puppet.Core.Flow;

namespace Puppet.API.Client
{
    public sealed class APILobby
    {
        public static void GetAllChannel(Action<bool, string, List<DataChannel>> onGetListChannel)
        {
            ScenePockerLobby.Instance.GetListChannel(onGetListChannel);
        }

        public static void GoToChannel(IGameType channel)
        {

        }
    }
}
