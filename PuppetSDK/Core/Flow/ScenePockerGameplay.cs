using Puppet.Core.Model;
using Puppet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Flow
{
    internal class ScenePockerGameplay : BaseSingleton<ScenePockerGameplay>, IScene
    {
        #region DEFAULT NOT MODIFY
        public string ServerScene
        {
            get { return "poker"; }
        }

        public string SceneName
        {
            get { return "PockerGameplay"; }
        }

        public EScene SceneType
        {
            get { return EScene.Pocker_Gameplay; }
        }

        public IScene NextScene
        {
            get { return ScenePockerLobby.Instance; }
        }

        public IScene PrevScene
        {
            get { return ScenePockerLobby.Instance; }
        }
        #endregion

        public void BeginScene()
        {
            if(ScenePockerLobby.Instance.lastUpdateGroupChildren != null)
            {
                foreach(DataLobby lobby in ScenePockerLobby.Instance.lastUpdateGroupChildren)
                {
                    if(lobby.roomId == RoomHandler.Instance.Current.Id)
                    {
                        PuGlobal.Instance.SelectedLobby = lobby;
                        break;
                    }
                }
            }
            PuMain.GameLogic.EnterGameplay();
        }

        public void EndScene()
        {
            PuMain.GameLogic.ExitGameplay();
        }

        protected override void Init()
        {
        }

        public void ProcessEvents(string eventType, Network.Socket.ISocketResponse onEventResponse)
        {
            PuMain.GameLogic.ProcessEvents(eventType, onEventResponse);
        }
    }
}
