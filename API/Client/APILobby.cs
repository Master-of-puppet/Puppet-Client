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

        public static void GetGroupChildren(string groupName, Action<bool, string, List<DataChannel>> onGetCallback)
        {
            ScenePockerLobby.Instance.GetGroupChildren(groupName, onGetCallback);
        }

        public static void CreateLobby(Action<bool, string, DataChannel> onCreateLobbyCallback)
        {

        }

        public static void AddListener(Action<List<DataChannel>> onCreateCallback, Action<List<DataChannel>> onUpdateCallback, Action<List<DataChannel>> onDeleteCallback)
        {

        }
        public static void RemoveListener(Action<List<DataChannel>> onCreateCallback, Action<List<DataChannel>> onUpdateCallback, Action<List<DataChannel>> onDeleteCallback)
        {

        }

        public static void GoToChannel(IGameType channel)
        {

        }
    }
}
