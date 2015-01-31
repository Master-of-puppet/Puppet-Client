using Puppet.Core.Flow;
using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.API.Client
{
    public sealed class APIWorldGame
    {
        /// <summary>
        /// Lấy danh sách các trò chơi tại thế giới trò chơi
        /// - Pocker
        /// - Chắn
        /// - Phỏm
        /// - TLMN
        /// - ....
        /// </summary>
        public static void GetListGame(DelegateAPICallbackDataGame onGetListGame)
        {
            if (SceneHandler.Instance.Current.SceneType != EScene.World_Game)
            {
                onGetListGame(false, "API chỉ được thực thi khi ở màn World Game", null);
                return;
            }

            //SceneWorldGame.Instance.GetListGame(onGetListGame);
        }

        public static void JoinRoom(DataGame game, DelegateAPICallback onJoinRoomCallback)
        {
            SceneWorldGame.Instance.JoinGame(game, onJoinRoomCallback);
        }
    }
}
