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

        /// <summary>
        /// Info Setting Poker Game
        /// </summary>
        public PokerGameDetails gameDetails;
        public event Action<PokerCard[]> onDealCardsChange;
        public event Action<DataMessageBase> onDispatchViewCard;
        public event Action<ResponseUpdateGame> onFirstTimeJoinGame;

        List<KeyValuePair<string, object>> queueWaitingSendClient;
        bool isClientWasListener;

        /// <summary>
        /// Thông tin người chơi trong game
        /// </summary>
        Dictionary<string, PokerPlayerController> _dictAllPlayers = new Dictionary<string,PokerPlayerController>();
        double _maxCurrentBetting = 0;
        SortedDictionary<string, int> _timesInteractiveInRound = new SortedDictionary<string, int>();
        List<string> _listPlayerInGame = new List<string>();
        List<string> _listPlayerWaitNextGame = new List<string>();
        string _dealerName = string.Empty;
        List<PokerCard> _comminityCards = new List<PokerCard>();
        List<PokerCard> _cardMainPlayer = new List<PokerCard>();
        string _roomMaster = string.Empty;
        double _lastBetForSitdown;
        bool firstTimeJoinGame = false;
        UserInfo _mUserInfo;

        public override void EnterGameplay()
        {
            _mUserInfo = Puppet.API.Client.APIUser.GetUserInformation();
            gameDetails = null;
            MainPlayer = LastPlayer = CurrentPlayer = null;
            isClientWasListener = false;
            queueWaitingSendClient = new List<KeyValuePair<string, object>>();
            _dictAllPlayers = new Dictionary<string, PokerPlayerController>();
            _maxCurrentBetting = 0;
            _listPlayerInGame = new List<string>();
            _listPlayerWaitNextGame = new List<string>();
            _lastBetForSitdown = 0;
            firstTimeJoinGame = true;
            PuMain.Dispatcher.onUserInfoUpdate += Dispatcher_onUserInfoUpdate;
        }

        public override void ExitGameplay() 
        {
            PuMain.Dispatcher.onUserInfoUpdate -= Dispatcher_onUserInfoUpdate;
        }

        void Dispatcher_onUserInfoUpdate(UserInfo userInfo)
        {
            string dictKey = userInfo.info.userName;
            if (_dictAllPlayers.ContainsKey(dictKey))
                _dictAllPlayers[dictKey].globalAsset.UpdateAssets(userInfo.assets.content);
        }

        public override void ProcessEvents(string eventType, ISocketResponse onEventResponse)
        {
            if(eventType == SFSEvent.EXTENSION_RESPONSE)
            {
                ISFSObject messageObj = Puppet.Utils.Utility.GetDataExtensionResponse(onEventResponse, Fields.RESPONSE_CMD_PLUGIN_MESSAGE);
                if(messageObj != null)
                {
                    string command = Puppet.Utils.Utility.GetCommandName(messageObj);
                    switch (command)
                    {
                        case "updateGameToWaitingPlayer":   /// Nhận được command khi lần đầu vào game (trạng thái là đang chờ người chơi khác)
                        case "updateGame":                  /// Nhận được command khi lần đầu vào game (nhưng trận đấu đang diễn ra)
                            ResponseUpdateGame dataUpdateGame = SFSDataModelFactory.CreateDataModel<ResponseUpdateGame>(messageObj);
                            if (dataUpdateGame != null && dataUpdateGame.gameDetails != null)
                            {
                                gameDetails = dataUpdateGame.gameDetails;
                                MAX_PLAYER_IN_GAME = gameDetails.customConfiguration.numPlayers;
                            }
                            RefreshDataPlayer(dataUpdateGame.players);
                            RefreshListPlayerInGame(false, dataUpdateGame.players);
                            UpdateCardDeal(dataUpdateGame.dealComminityCards);
                            DispathToClient(command, dataUpdateGame);
                            break;
                        case "updateGameState":             /// Nhận được command khi gameState thay đổi
                            ResponseUpdateGameState dataGameState = SFSDataModelFactory.CreateDataModel<ResponseUpdateGameState>(messageObj);
                            DispathToClient(command, dataGameState);
                            break;
                        case "updateRoomMaster":            /// Nhận được command khi chủ phòng bị thay đổi
                            ResponseUpdateRoomMaster dataRoomMaster = SFSDataModelFactory.CreateDataModel<ResponseUpdateRoomMaster>(messageObj);
                            _roomMaster = dataRoomMaster.player.userName;
                            RefreshDataPlayer(dataRoomMaster.player);
                            DispathToClient(command, dataRoomMaster);
                            break;
                        case "playerListChanged":          /// Nhận được command khi có người chơi thay đổi trong phòng
                            ResponsePlayerListChanged dataPlayerListChanged = SFSDataModelFactory.CreateDataModel<ResponsePlayerListChanged>(messageObj);
                            UpdatePlayerInRoom(dataPlayerListChanged);
                            DispathToClient(command, dataPlayerListChanged);
                            break;
                        case "updateHand":                  /// Nhận được command khi bắt đầu chia bài
                            ResponseUpdateHand dataUpdateHand = SFSDataModelFactory.CreateDataModel<ResponseUpdateHand>(messageObj);
                            UpdateCardMainPlayer(dataUpdateHand.hand);
                            RefreshDataPlayer(dataUpdateHand.players);
                            RefreshListPlayerInGame(true, dataUpdateHand.players);
                            HandleUpdateHand(dataUpdateHand.dealer);
                            DispathToClient(command, dataUpdateHand);
                            break;
                        case "turn":                        /// Nhận được command khi chuyển đến lượt chơi của người nào đó
                            ResponseUpdateTurnChange dataTurn = SFSDataModelFactory.CreateDataModel<ResponseUpdateTurnChange>(messageObj);
                            UpdatePlayerData(dataTurn);
                            UpdateCardDeal(dataTurn.dealComminityCards);
                            RefreshDataPlayer(dataTurn.fromPlayer, dataTurn.toPlayer);
                            DispathToClient(command, dataTurn);
                            break;
                        case "finishGame":                  /// Nhận được command khi ván chơi kết thúc
                            ResponseFinishGame dataFinishGame = SFSDataModelFactory.CreateDataModel<ResponseFinishGame>(messageObj);
                            UpdateCardDeal(dataFinishGame.dealComminityCards);
                            HandleFinishGame(dataFinishGame);
                            DispathToClient(command, dataFinishGame);
                            break;
                        case "waitingDealCard":             /// Nhận được command khi 
                            ResponseWaitingDealCard dataWaitingDealcard = SFSDataModelFactory.CreateDataModel<ResponseWaitingDealCard>(messageObj);
                            HandleWaitNewGame();
                            DispathToClient(command, dataWaitingDealcard);
                            break;
                        case "udpatePot":                   /// Nhận được command khi có một pot thay đổi giá trị
                            ResponseUpdatePot dataPot = SFSDataModelFactory.CreateDataModel<ResponseUpdatePot>(messageObj);
                            ResetCurrentBetting();
                            DispathToClient(command, dataPot);
                            break;
                        case "updateUserInfo":             /// Nhận được command khi thông tin của người chơi trong bàn có cập nhật
                            ResponseUpdateUserInfo dataUserInfo = SFSDataModelFactory.CreateDataModel<ResponseUpdateUserInfo>(messageObj);
                            UpdateUserInfo(dataUserInfo);
                            DispathToClient(command, dataUserInfo);
                            break;
                        case "error":                      /// Nhận được command khi có mỗi xẩy ra trong gameplay
                            ResponseError dataError = SFSDataModelFactory.CreateDataModel<ResponseError>(messageObj);
                            DispathToClient(command, dataError);
                            break;
                    }
                }
            }
            else if (eventType.Equals(SFSEvent.PUBLIC_MESSAGE))
            {
                if (onEventResponse.Params.Contains(Fields.MESSAGE) && onEventResponse.Params[Fields.MESSAGE].ToString() == Constant.KEY_VIEW_CARD)
                {
                    ISFSObject obj = (SFSObject)onEventResponse.Params[Fields.DATA];
                    DataMessageBase dataViewCard = SFSDataModelFactory.CreateDataModel<DataMessageBase>(obj);
                    if (onDispatchViewCard != null)
                        onDispatchViewCard(dataViewCard);
                }
            }
        }

        #region Dispath To Client
        private void DispathToClient(string command, object data)
        {
            if (isClientWasListener)
                SendGameEventToClient(command, data);
            else
                queueWaitingSendClient.Add(new KeyValuePair<string, object>(command, data));
        }

        public void StartListenerEvent()
        {
            foreach (KeyValuePair<string, object> keyAndValue in queueWaitingSendClient)
                SendGameEventToClient(keyAndValue.Key, keyAndValue.Value);
            queueWaitingSendClient.Clear();
            isClientWasListener = true;
        }

        void SendGameEventToClient(string command, object data)
        {
            if (firstTimeJoinGame && data is ResponseUpdateGame)
            {
                firstTimeJoinGame = false;
                ResponseUpdateGame dataGame = data as ResponseUpdateGame;
                HandleFirstTimeJoinGame(dataGame);
                if (onFirstTimeJoinGame != null)
                    onFirstTimeJoinGame(dataGame);
            }
            else
            {
                EventDispatcher.SetGameEvent(command, data);
            }
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

        #region Card Hand & Comminity Card
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
                for (int i = 0; i < cards.Length;i++ )
                    _cardMainPlayer.Add(new PokerCard(cards[i]));
            }
        }

        void HandleUpdateHand(string userName)
        {
            this._dealerName = userName;
        }

        void HandleWaitNewGame()
        {
            _comminityCards.Clear();
            _cardMainPlayer.Clear();
        }

        public List<PokerCard> DealComminityCards { get { return _comminityCards; } }

        public List<PokerCard> CardsMainPlayer { get { return _cardMainPlayer; } }
        #endregion

        #region Betting
        public double MaxCurrentBetting
        {
            get { return _maxCurrentBetting; }
            private set { if (_maxCurrentBetting < value) _maxCurrentBetting = value; }
        }

        void ResetCurrentBetting()
        {
            foreach (string userName in _timesInteractiveInRound.Keys)
                _timesInteractiveInRound[userName] = 0;
            _maxCurrentBetting = 0;
            ListPlayer.ForEach(p => { p.currentBet = 0; p.DispatchAttribute("currentBet"); });
        }

        public int GetTimesInteractiveInRound(string userName)
        {
            return _timesInteractiveInRound.ContainsKey(userName) ? _timesInteractiveInRound[userName] : 0;
        }

        void HandleFinishGame(ResponseFinishGame dataFinishGame)
        {
            ResetCurrentBetting();
        }

        void HandleFirstTimeJoinGame(ResponseUpdateGame dataGame)
        {
            if (!string.IsNullOrEmpty(dataGame.dealer))
                HandleUpdateHand(dataGame.dealer);

            if(dataGame.players != null)
            {
                PokerPlayerController player = Array.Find<PokerPlayerController>(dataGame.players, p => p.inTurn);
                if (player != null)
                    CurrentPlayer = (PokerPlayerController)player.CloneObject();

                for(int i=0;i<dataGame.players.Length;i++)
                    MaxCurrentBetting = dataGame.players[i].currentBet;

                ListPlayer.ForEach(p => p.DispatchAttribute(string.Empty));
            }
        }

        void UpdatePlayerData(ResponseUpdateTurnChange dataTurn)
        {
            if (dataTurn != null)
            {
                if (dataTurn.toPlayer != null)
                    CurrentPlayer = (PokerPlayerController)dataTurn.toPlayer.CloneObject();
                if (dataTurn.fromPlayer != null)
                {
                    string fromUsername = dataTurn.fromPlayer.userName;
                    if (!_timesInteractiveInRound.ContainsKey(fromUsername))
                        _timesInteractiveInRound.Add(fromUsername, 0);
                    _timesInteractiveInRound[fromUsername]++;

                    LastPlayer = (PokerPlayerController)dataTurn.fromPlayer.CloneObject();
                }

                if (CurrentPlayer != null)
                    MaxCurrentBetting = CurrentPlayer.currentBet;
                if (LastPlayer != null)
                    MaxCurrentBetting = LastPlayer.currentBet;
                if (dataTurn.firstTurn && dataTurn.bigBlind != null)
                    MaxCurrentBetting = dataTurn.bigBlind.currentBet;
            }
        }
        #endregion

        #region updateUserInfo
        private void UpdateUserInfo(ResponseUpdateUserInfo dataUserInfo)
        {
            if (dataUserInfo.asset == null) return;

            if (dataUserInfo.field == "assetGame") //dataUserInfo.field == "experience"
            {
                PokerPlayerController player = GetPlayer(dataUserInfo.userName);
                if (player != null)
                {
                    player.asset.UpdateAssets(dataUserInfo.asset.content);
                    RefreshDataPlayer(player);
                }
            }
            else if (dataUserInfo.field == "asset") 
            {
                // Lưu ngược vào SDK Core. Sau này nên xử lý riêng ở Core không nên đưa vào Gameplay.
                API.Client.APIUser.UpdateAssetInfo(dataUserInfo.asset.content);
            }
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

                    if (ListPlayerInGame.Count == 0)
                        EndFinishGame();

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
                        _dictAllPlayers[p.userName].UpdateData(p, false, false);

                    if (_dictAllPlayers[p.userName].userName == _mUserInfo.info.userName)
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
                    if (p != null && players.Length > 0
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
            CurrentPlayer = LastPlayer = null;
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

        internal void StandUp()
        {
            _lastBetForSitdown = 0;
            PuMain.Socket.Request(RequestPool.GetStandUpRequest());
        }

        internal void AutoSitDown()
        {
            List<int> listIndex = (from player in ListPlayer select player.slotIndex).ToList<int>();
            int minValue = -1;
            for (int i = 0; i < MAX_PLAYER_IN_GAME; i++)
                if (listIndex.Contains(i) == false) { minValue = i; break; }
            
            if (minValue >= 0)
                API.Client.APIPokerGame.SitDown(minValue, LastBetForSitdown);
        }
        #endregion

        #region Others
        public string Dealer { get { return _dealerName; } }

        public bool CanBePlay
        {
            get
            {
                double currentChip = Puppet.API.Client.APIUser.GetUserInformation().assets.GetAsset(EAssets.Chip).value;
                return currentChip >= MinimumChipToPlay;
            }
        }

        public double MinimumChipToPlay { get { return SmallBlind * 20f; } }

        public double SmallBlind { get { return gameDetails.customConfiguration.SmallBlind; } }

        public double MaxBlind { get { return gameDetails.customConfiguration.SmallBlind * 2; } }

        public double LastBetForSitdown
        {
            get
            {
                if (_lastBetForSitdown <= 0)
                    _lastBetForSitdown = SmallBlind * 20;
                return _lastBetForSitdown;
            }
        }

        public string RoomMaster { get { return _roomMaster; } }

        public bool IsMainPlayer(string userName)
        {
            return _mUserInfo.info.userName == userName;
        }

        public bool IsMainPlayerInGame
        {
            get { return IsPlayerInGame(_mUserInfo.info.userName); }
        }

        public bool IsPlayerInGame(string userName)
        {
            return ListPlayerInGame.Contains(userName);
        }

        public bool IsMainPlayerSatDown
        {
            get { return ListPlayerInGame.Find(userName => userName == _mUserInfo.info.userName) != null; }
        }

        public bool IsMainTurn
        {
            get { try { return CurrentPlayer.userName == MainPlayer.userName; } catch { return false; } }
        }

        public int GetTotalPlayerNotFold
        {
            get { return ListPlayer.FindAll(p => p.GetPlayerState() != PokerPlayerState.fold && p.GetPlayerState() != PokerPlayerState.none).Count; }
        }

        public double CurrentBettingDiff
        {
            get
            {
                if (CurrentPlayer == null)
                    return 0;
                double leftMoney = CurrentPlayer.GetMoney();
                double diff = MaxCurrentBetting - MainPlayer.currentBet;
                return leftMoney > diff ? diff : leftMoney;
            }
        }

        public UserInfo mUserInfo
        {
            get { return _mUserInfo; }
        }
        #endregion
    }
}
