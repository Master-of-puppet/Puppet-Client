using Puppet.Core.Model;
using Puppet.Core.Model.Factory;
using Puppet.Core.Network.Socket;
using Puppet.Poker.Datagram;
using Puppet.Poker.Models;
using Puppet.Utils;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Poker
{
    public class PokerGameplay : AbstractGameplay<PokerPlayerController>
    {
        public const PokerSide DEFAULT_SIDE_MAIN_PLAYER = PokerSide.Slot_1;
        public int MAX_PLAYER_IN_GAME = 9;

        public ResponseUpdateGame dataUpdateGame;
        public ResponseUpdateRoomMaster dataRoomMaster;
        public ResponseUpdateGameState dataGameState;

        public override void EnterGameplay()
        {
        }

        public override void ExitGameplay()
        {

        }

        public override void ProcessEvents(string eventType, ISocketResponse onEventResponse)
        {
            if(eventType == SFSEvent.EXTENSION_RESPONSE)
            {
                ISFSObject messageObj = Puppet.Utils.Utility.GetDataExtensionResponse(onEventResponse, Fields.RESPONSE_CMD_PLUGIN_MESSAGE);
                if(messageObj != null)
                {
                    string command = messageObj.GetUtfString("command");
                    switch (command)
                    {
                        case "updateGameToWaitingPlayer":
                        case "updateGame":
                            dataUpdateGame = SFSDataModelFactory.CreateDataModel<ResponseUpdateGame>(messageObj);
                            EventDispatcher.SetGameEvent(command, dataUpdateGame);
                            break;
                        case "updateGameState":
                            dataGameState = SFSDataModelFactory.CreateDataModel<ResponseUpdateGameState>(messageObj);
                            EventDispatcher.SetGameEvent(command, dataGameState);
                            break;
                        case "updateRoomMaster":
                            dataRoomMaster = SFSDataModelFactory.CreateDataModel<ResponseUpdateRoomMaster>(messageObj);
                            EventDispatcher.SetGameEvent(command, dataRoomMaster);
                            break;
                    }
                }
            }
        }

        public override void PlayerEnter(PokerPlayerController player)
        {
        }

        public override void PlayerExit(PokerPlayerController player)
        {
        }

        public override void BeginGameRound()
        {
        }

        public override void EndGameRound()
        {
        }

        public override void ChangeState(string oldState, string newState)
        {
        }


        public List<PokerPlayerController> ListPlayer
        {
            get { return new List<PokerPlayerController>(dataUpdateGame.players); }
        }

        PokerPlayerController _yourController;
        public PokerPlayerController YourController
        {
            get 
            {
                if(_yourController == null)
                    _yourController = ListPlayer.Find(p => p.userName == Puppet.API.Client.APIUser.GetUserInformation().info.userName);
                return _yourController;
            }
            set { _yourController = value; }
        }

        public PokerSide GetSide(PokerPlayerController player)
        {
            PokerPlayerController your = YourController;
            if (your == null || your.slotIndex == (int)DEFAULT_SIDE_MAIN_PLAYER || your.slotIndex >= MAX_PLAYER_IN_GAME)
                return (PokerSide)player.slotIndex;
            
            int slot;

            if (player.slotIndex > your.slotIndex)
                slot = player.slotIndex - your.slotIndex;
            else
                slot = MAX_PLAYER_IN_GAME - your.slotIndex + player.slotIndex;

            slot += (int)DEFAULT_SIDE_MAIN_PLAYER;
            if (slot >= MAX_PLAYER_IN_GAME)
                slot -= (MAX_PLAYER_IN_GAME - 1);

            return (PokerSide)slot;
        }

        public int GetSlotServer(PokerSide side)
        {
            return (int)side;
        }

        internal void SendSitDown(PokerSide side)
        {
            PuMain.Socket.Request(Puppet.Poker.Datagram.RequestPool.GetSitRequest(RequestPlay.SIT, GetSlotServer(side)));
        }

        internal void SendPlayRequest(RequestPlay request, long value)
        {
            PuMain.Socket.Request(Puppet.Poker.Datagram.RequestPool.GetPlayRequest(request, value));
        }
    }
}
