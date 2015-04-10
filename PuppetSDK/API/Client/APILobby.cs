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

        /// <summary>
        /// Chọn và tham gia vào một kênh
        /// </summary>
        /// <param name="channel">Kênh muốn tham gia</param>
        /// <param name="onGetCallback">Thông tin phòng chơi trong kênh</param>
        public static void SetSelectChannel(DataChannel channel, DelegateAPICallbackDataLobby onGetCallback)
        {
            ScenePockerLobby.Instance.SetSelectChannel(channel, onGetCallback);
        }

        public static void CreateLobby(double maxBet, int numberPlayer, DelegateAPICallback onCreateLobbyCallback)
        {
            ScenePockerLobby.Instance.CreateLobby(maxBet, numberPlayer, onCreateLobbyCallback);
        }

        public static void JoinLobby(DataLobby lobby, DelegateAPICallback onJoinLobby)
        {
            ScenePockerLobby.Instance.JoinLobby(lobby, onJoinLobby);
        }

        public static void QuickJoinLobby(DelegateAPICallbackObject onJoinLobby)
        {
            ScenePockerLobby.Instance.QuickJoinLobby(onJoinLobby);
        }

        public static void AddListener(Action<DataLobby> onCreateCallback, Action<DataLobby> onUpdateCallback, Action<DataLobby> onDeleteCallback)
        {
            ScenePockerLobby.Instance.AddListener(onCreateCallback, onUpdateCallback, onDeleteCallback);
        }
        public static void RemoveListener()
        {
            ScenePockerLobby.Instance.RemoveListener();
        }
    }
}
