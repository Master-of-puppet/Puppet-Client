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

        public static void GetAllGroupName(DelegateAPICallbackObject onGetGroupNameCallback)
        {
            ScenePockerLobby.Instance.GetAllGroupName(onGetGroupNameCallback);
        }

        public static void SetSelectGroup(string groupName, DelegateAPICallbackDataLobby onGetCallback)
        {
            ScenePockerLobby.Instance.SetSelectGroup(groupName, onGetCallback);
        }

        public static void CreateLobby(DelegateAPICallbackDataLobby onCreateLobbyCallback)
        {

        }

        public static void AddListener(Action<List<DataLobby>> onCreateCallback, Action<List<DataLobby>> onUpdateCallback, Action<List<DataLobby>> onDeleteCallback)
        {

        }
        public static void RemoveListener(Action<List<DataLobby>> onCreateCallback, Action<List<DataLobby>> onUpdateCallback, Action<List<DataLobby>> onDeleteCallback)
        {

        }

        public static void GoToLobby(IGameType Lobby)
        {

        }
    }
}
