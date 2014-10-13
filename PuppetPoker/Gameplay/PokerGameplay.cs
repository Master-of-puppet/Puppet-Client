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

        internal ResponseUpdateGame dataUpdateGame;
        internal ResponseUpdateRoomMaster dataRoomMaster;
        internal ResponseUpdateGameState dataGameState;
        internal ResponsePlayerListChanged dataPlayerListChanged;

        List<KeyValuePair<string, object>> queueWaitingSendClient;
        bool isClientWasListener;

        public override void EnterGameplay()
        {
            isClientWasListener = false;
            queueWaitingSendClient = new List<KeyValuePair<string, object>>();
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
                            DispathToClient(command, dataUpdateGame);
                            break;
                        case "updateGameState":
                            dataGameState = SFSDataModelFactory.CreateDataModel<ResponseUpdateGameState>(messageObj);
                            DispathToClient(command, dataGameState);
                            break;
                        case "updateRoomMaster":
                            dataRoomMaster = SFSDataModelFactory.CreateDataModel<ResponseUpdateRoomMaster>(messageObj);
                            DispathToClient(command, dataRoomMaster);
                            break;
                        case "playerListChanged" :
                            dataPlayerListChanged = SFSDataModelFactory.CreateDataModel<ResponsePlayerListChanged>(messageObj);
                            DispathToClient(command, dataPlayerListChanged);
                            break;
                        case "updateHand":
                            ResponseUpdateHand dataUpdateHand = SFSDataModelFactory.CreateDataModel<ResponseUpdateHand>(messageObj);
                            DispathToClient(command, dataUpdateHand);
                            break;
                        case "turn":
                            ResponseUpdateTurnChange dataTurn = SFSDataModelFactory.CreateDataModel<ResponseUpdateTurnChange>(messageObj);
                            DispathToClient(command, dataTurn);
                            break;
                        case "finishGame":
                            ResponseFinishGame dataFinishGame = SFSDataModelFactory.CreateDataModel<ResponseFinishGame>(messageObj);
                            DispathToClient(command, dataFinishGame);
                            break;
                        case "waitingDealCard":
                            ResponseWaitingDealCard dataWaitingDealcard = SFSDataModelFactory.CreateDataModel<ResponseWaitingDealCard>(messageObj);
                            DispathToClient(command, dataWaitingDealcard);
                            break;
                    }
                }
            }
        }

        private void DispathToClient(string command, object data)
        {
            if (isClientWasListener)
                EventDispatcher.SetGameEvent(command, data);
            else
                queueWaitingSendClient.Add(new KeyValuePair<string, object>(command, data));
        }

        internal void StartListenerEvent()
        {
            foreach (KeyValuePair<string, object> keyAndValue in queueWaitingSendClient)
                EventDispatcher.SetGameEvent(keyAndValue.Key, keyAndValue.Value);
            queueWaitingSendClient.Clear();

            isClientWasListener = true;
        }

        public List<PokerPlayerController> ListPlayer
        {
            get { return new List<PokerPlayerController>(dataUpdateGame.players); }
        }

        public PokerPlayerController YourController
        {
            get 
            {
                return ListPlayer.Find(p => p.userName == Puppet.API.Client.APIUser.GetUserInformation().info.userName);
            }
        }

        internal PokerSide GetSide(PokerPlayerController player)
        {
            PokerPlayerController your = YourController;
            if (your == null || your.slotIndex == (int)DEFAULT_SIDE_MAIN_PLAYER || your.slotIndex >= MAX_PLAYER_IN_GAME)
                return (PokerSide)player.slotIndex;

            if (your.userName == player.userName)
                return DEFAULT_SIDE_MAIN_PLAYER;

            int slot = your.slotIndex;
            if (player.slotIndex > your.slotIndex)
                slot = player.slotIndex - your.slotIndex;
            else
                slot = MAX_PLAYER_IN_GAME - your.slotIndex + player.slotIndex;

            slot += (int)DEFAULT_SIDE_MAIN_PLAYER;
            if (slot >= MAX_PLAYER_IN_GAME)
                slot -= (MAX_PLAYER_IN_GAME - 1);

            return (PokerSide)slot;
        }

        internal void SendSitDown(int slotIndex)
        {
            PuMain.Socket.Request(Puppet.Poker.Datagram.RequestPool.GetSitRequest(PokerRequestPlay.SIT, slotIndex));
        }

        internal void SendPlayRequest(PokerRequestPlay request, long value)
        {
            PuMain.Socket.Request(Puppet.Poker.Datagram.RequestPool.GetPlayRequest(request, value));
        }
    }
}
