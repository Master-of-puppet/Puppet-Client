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
using System.Timers;

namespace Puppet.Poker
{
    public class PokerGameplay : AbstractGameplay<PokerPlayerController>
    {
        public const PokerSide DEFAULT_SIDE_MAIN_PLAYER = PokerSide.Slot_1;
        public int MAX_PLAYER_IN_GAME = 9;

        public event Action<PokerCard[]> onDealCardsChange;

        List<KeyValuePair<string, object>> queueWaitingSendClient;
        bool isClientWasListener;
        /// <summary>
        /// Thông tin người chơi trong game
        /// </summary>
        Dictionary<string, PokerPlayerController> _dictAllPlayers = new Dictionary<string,PokerPlayerController>();
        double _maxCurrentBetting = 0;
        List<string> _listPlayerInGame = new List<string>();
        List<string> _listPlayerWaitNextGame = new List<string>();
        string mainPlayerUsername = string.Empty;
        string _dealerName = string.Empty;
        List<PokerCard> _comminityCards = new List<PokerCard>();
        List<PokerCard> _cardMainPlayer = new List<PokerCard>();

        public override void EnterGameplay()
        {
            MainPlayer = LastPlayer = CurrentPlayer = null;
            isClientWasListener = false;
            queueWaitingSendClient = new List<KeyValuePair<string, object>>();
            _dictAllPlayers = new Dictionary<string, PokerPlayerController>();
            _maxCurrentBetting = 0;
            _listPlayerInGame = new List<string>();
            _listPlayerWaitNextGame = new List<string>();
            mainPlayerUsername = Puppet.API.Client.APIUser.GetUserInformation().info.userName;
        }

        public override void ExitGameplay() {}

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
                            ResponseUpdateGame dataUpdateGame = SFSDataModelFactory.CreateDataModel<ResponseUpdateGame>(messageObj);
                            RefreshDataPlayer(dataUpdateGame.players);
                            RefreshListPlayerInGame(false, dataUpdateGame.players);
                            UpdateCardDeal(dataUpdateGame.dealComminityCards);
                            DispathToClient(command, dataUpdateGame);
                            break;
                        case "updateGameState":
                            ResponseUpdateGameState dataGameState = SFSDataModelFactory.CreateDataModel<ResponseUpdateGameState>(messageObj);
                            DispathToClient(command, dataGameState);
                            break;
                        case "updateRoomMaster":
                            ResponseUpdateRoomMaster dataRoomMaster = SFSDataModelFactory.CreateDataModel<ResponseUpdateRoomMaster>(messageObj);
                            RefreshDataPlayer(dataRoomMaster.player);
                            DispathToClient(command, dataRoomMaster);
                            break;
                        case "playerListChanged" :
                            ResponsePlayerListChanged dataPlayerListChanged = SFSDataModelFactory.CreateDataModel<ResponsePlayerListChanged>(messageObj);
                            UpdatePlayerInRoom(dataPlayerListChanged);
                            DispathToClient(command, dataPlayerListChanged);
                            break;
                        case "updateHand":
                            ResponseUpdateHand dataUpdateHand = SFSDataModelFactory.CreateDataModel<ResponseUpdateHand>(messageObj);
                            UpdateCardMainPlayer(dataUpdateHand.hand);
                            RefreshDataPlayer(dataUpdateHand.players);
                            RefreshListPlayerInGame(true, dataUpdateHand.players);
                            HandleUpdateHand(dataUpdateHand);
                            DispathToClient(command, dataUpdateHand);
                            break;
                        case "turn":
                            ResponseUpdateTurnChange dataTurn = SFSDataModelFactory.CreateDataModel<ResponseUpdateTurnChange>(messageObj);
                            UpdatePlayerData(dataTurn);
                            UpdateCardDeal(dataTurn.dealComminityCards);
                            RefreshDataPlayer(dataTurn.fromPlayer, dataTurn.toPlayer);
                            DispathToClient(command, dataTurn);
                            break;
                        case "finishGame":
                            ResponseFinishGame dataFinishGame = SFSDataModelFactory.CreateDataModel<ResponseFinishGame>(messageObj);
                            UpdateCardDeal(dataFinishGame.dealComminityCards);
                            HandleFinishGame(dataFinishGame);
                            DispathToClient(command, dataFinishGame);
                            break;
                        case "waitingDealCard":
                            ResponseWaitingDealCard dataWaitingDealcard = SFSDataModelFactory.CreateDataModel<ResponseWaitingDealCard>(messageObj);
                            HandleWaitNewGame();
                            DispathToClient(command, dataWaitingDealcard);
                            break;
                        case "udpatePot":
                            ResponseUpdatePot dataPot = SFSDataModelFactory.CreateDataModel<ResponseUpdatePot>(messageObj);
                            ResetCurrentBetting();
                            DispathToClient(command, dataPot);
                            break;
                        case "updateUserInfo" :
                            ResponseUpdateUserInfo dataUserInfo = SFSDataModelFactory.CreateDataModel<ResponseUpdateUserInfo>(messageObj);
                            RefreshDataPlayer(dataUserInfo.userInfo);
                            DispathToClient(command, dataUserInfo);
                            break;
                        case "error" :
                            ResponseError dataError = SFSDataModelFactory.CreateDataModel<ResponseError>(messageObj);
                            DispathToClient(command, dataError);
                            break;
                    }
                }
            }
        }

        #region Dispath To Client
        private void DispathToClient(string command, object data)
        {
            if (isClientWasListener)
                EventDispatcher.SetGameEvent(command, data);
            else
                queueWaitingSendClient.Add(new KeyValuePair<string, object>(command, data));
        }

        public void StartListenerEvent()
        {
            foreach (KeyValuePair<string, object> keyAndValue in queueWaitingSendClient)
                EventDispatcher.SetGameEvent(keyAndValue.Key, keyAndValue.Value);
            queueWaitingSendClient.Clear();
            isClientWasListener = true;
        }
        public void StartFinishGame()
        {
            isClientWasListener = false;
        }
        public void EndFinishGame()
        {
            StartListenerEvent();
            ResetListPlayerInGame();
        }
        #endregion

        void UpdateCardDeal(int [] comminityCards)
        {
            if (comminityCards == null || comminityCards.Length == 0) return;

            List<PokerCard> newCards = new List<PokerCard>();
            for (int i = 0; i < comminityCards.Length; i++)
            {
                int cardId = comminityCards[i];
                if (_comminityCards.Find(c => c.cardId == cardId) != null || newCards.Find(c => c.cardId ==cardId) != null)
                    continue;

                newCards.Add(new PokerCard(cardId));
            }

            if (newCards.Count > 0)
            {
                _comminityCards.AddRange(newCards);
                if (onDealCardsChange != null)
                    onDealCardsChange(newCards.ToArray());
            }
        }
        void UpdateCardMainPlayer(int [] cards)
        {
            if(cards != null && cards.Length > 0)
            {
                _cardMainPlayer.Clear();
                for (int i = 0; i < cards.Length;i++ )
                {
                    int cardId = cards[i];
                    _cardMainPlayer = new List<PokerCard>(cardId);
                }
            }
        }

        void HandleUpdateHand(ResponseUpdateHand dataUpdateHand)
        {
            this._dealerName = dataUpdateHand.dealer;
        }

        void HandleWaitNewGame()
        {
            _comminityCards.Clear();
            _cardMainPlayer.Clear();
        }

        public string Dealer { get { return _dealerName; } }

        public List<PokerCard> DealComminityCards { get { return _comminityCards; } }

        public List<PokerCard> CardsMainPlayer { get { return _cardMainPlayer; } }

        #region Betting
        public double MaxCurrentBetting
        {
            get { return _maxCurrentBetting; }
            private set { if (_maxCurrentBetting < value) _maxCurrentBetting = value; }
        }

        void HandleFinishGame(ResponseFinishGame dataFinishGame)
        {
        }

        void UpdatePlayerData(ResponseUpdateTurnChange dataTurn)
        {
            if (dataTurn != null)
            {
                if (dataTurn.toPlayer != null)
                    CurrentPlayer = (PokerPlayerController)dataTurn.toPlayer.CloneObject();
                if(dataTurn.fromPlayer != null)
                    LastPlayer = (PokerPlayerController)dataTurn.fromPlayer.CloneObject();

                if (CurrentPlayer != null)
                    MaxCurrentBetting = CurrentPlayer.currentBet;
                if (LastPlayer != null)
                    MaxCurrentBetting = LastPlayer.currentBet;
                if (dataTurn.firstTurn && dataTurn.bigBlind != null)
                    MaxCurrentBetting = dataTurn.bigBlind.currentBet;
            }
        }
        
        void ResetCurrentBetting()
        {
            _maxCurrentBetting = 0;
            ListPlayer.ForEach(p => { p.currentBet = 0; p.DispatchAttribute("currentBet"); });
        }
        #endregion

        #region Player Controller
        void UpdatePlayerInRoom(ResponsePlayerListChanged dataPlayerChange)
        {
            switch (dataPlayerChange.GetActionState())
            {
                case PokerPlayerChangeAction.playerAdded:
                    RefreshDataPlayer(dataPlayerChange.player);
                    if (ListPlayerInGame.Count > 0)
                        _listPlayerWaitNextGame.Add(dataPlayerChange.player.userName);
                    break;
                case PokerPlayerChangeAction.playerRemoved:
                case PokerPlayerChangeAction.playerQuitGame:
                    _dictAllPlayers.Remove(dataPlayerChange.player.userName);
                    _listPlayerInGame.Remove(dataPlayerChange.player.userName);
                    _listPlayerWaitNextGame.Remove(dataPlayerChange.player.userName);
                    break;
            }
        }

        void RefreshDataPlayer(params PokerPlayerController [] players)
        {
            if (players != null && players.Length > 0)
            {
                foreach (PokerPlayerController p in players)
                {
                    if (p == null) 
                        continue;

                    if (!_dictAllPlayers.ContainsKey(p.userName))
                        _dictAllPlayers.Add(p.userName, p);
                    else
                        _dictAllPlayers[p.userName].UpdateData(p, false);

                    if (_dictAllPlayers[p.userName].userName == mainPlayerUsername)
                        MainPlayer = _dictAllPlayers[p.userName];
                }

                ListPlayer.ForEach(p => {
                    if (p != null) p.DispatchAttribute(string.Empty);
                });
            }
        }

        void RefreshListPlayerInGame(bool ignoreState, params PokerPlayerController[] players)
        {
            if (players != null && players.Length > 0)
            {
                Array.ForEach<PokerPlayerController>(players, p =>
                {
                    if (p != null 
                        && (ignoreState || p.GetPlayerState() != PokerPlayerState.none)
                        && !_listPlayerInGame.Contains(p.userName))
                        _listPlayerInGame.Add(p.userName);
                });
            }
        }
        void ResetListPlayerInGame()
        {
            _listPlayerInGame.Clear();
            _listPlayerWaitNextGame.Clear();
            ListPlayer.ForEach(p => { if (p != null) p.DispatchAttribute(string.Empty); });
        }

        public List<PokerPlayerController> ListPlayer 
        { 
            get { return _dictAllPlayers.Select(p => p.Value).ToList(); } 
        }

        public List<string> ListPlayerInGame
        {
            get { return _listPlayerInGame; }
        }

        public List<string> ListPlayerWaitNextGame
        {
            get { return _listPlayerWaitNextGame; }
        }

        public PokerPlayerController GetPlayer(string userName)
        {
            return ListPlayer.Find(p => p.userName == userName);
        }

        public bool IsMainPlayerInGame
        {
            get { return IsPlayerInGame(mainPlayerUsername); }
        }

        public bool IsPlayerInGame(string userName)
        {
            return ListPlayerInGame.Contains(userName);
        }

        public PokerPlayerController MainPlayer
        {
            get;
            private set;
        }

        public PokerPlayerController LastPlayer
        {
            get; 
            private set;
        }

        public PokerPlayerController CurrentPlayer
        {
            get;
            private set;
        }
        #endregion

        #region Action Command
        internal PokerSide GetSide(PokerPlayerController player)
        {
            PokerPlayerController your = GetPlayer(Puppet.API.Client.APIUser.GetUserInformation().info.userName);
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

        internal void AutoSitDown(double money)
        {
            List<int> listIndex = (from player in ListPlayer select player.slotIndex).ToList<int>();
            int minValue = -1;
            for (int i = 0; i < Puppet.API.Client.APIPokerGame.GetPokerGameplay().MAX_PLAYER_IN_GAME; i++)
                if (listIndex.Contains(i) == false) { minValue = i; break; }

            if (minValue >= 0)
                API.Client.APIPokerGame.SitDown(minValue, money);
        }
        #endregion
    }
}
