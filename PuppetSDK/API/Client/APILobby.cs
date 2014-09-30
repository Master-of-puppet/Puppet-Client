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
        public static void GetAllLobby(DelegateAPICallbackDataLobby onGetAllLobby)
        {
            ScenePockerLobby.Instance.GetAllLobby(onGetAllLobby);
        }

        public static void GetGroupsLobby(DelegateAPICallbackDataChannel onGetGroupNameCallback)
        {
            ScenePockerLobby.Instance.GetGroupsLobby(onGetGroupNameCallback);
        }

        public static void SetSelectChannel(DataChannel channel, DelegateAPICallbackDataLobby onGetCallback)
        {
            ScenePockerLobby.Instance.SetSelectChannel(channel, onGetCallback);
        }

        public static void CreateLobby(DelegateAPICallback onCreateLobbyCallback)
        {
            ScenePockerLobby.Instance.CreateLobby(onCreateLobbyCallback);
        }

        public static void JoinLobby(DataLobby lobby, DelegateAPICallback onJoinLobby)
        {
            ScenePockerLobby.Instance.JoinLobby(lobby, onJoinLobby);
        }

        public static void AddListener(Action<List<DataLobby>> onCreateCallback, Action<List<DataLobby>> onUpdateCallback, Action<List<DataLobby>> onDeleteCallback)
        {

        }
        public static void RemoveListener(Action<List<DataLobby>> onCreateCallback, Action<List<DataLobby>> onUpdateCallback, Action<List<DataLobby>> onDeleteCallback)
        {

        }
    }
}
