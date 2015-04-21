using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Puppet.Poker.Models
{
    public class PokerPlayerController : DataPlayerController
    {
        public double currentBet { get; set; }
        public int handSize { get; set; }
        public string action { get; set; }
        public int[] hand { get; set; }
        public bool inTurn { get; set; }
        public DataAssets globalAsset { get; set; }

        private DataUser _globalInfo;

        public PokerPlayerController() 
            : base()
        {
        }

        public PokerSide GetSide()
        {
            return PokerMain.Instance.game.GetSide(this);
        }

        public PokerPlayerState GetPlayerState()
        {
            if (string.IsNullOrEmpty(gameState))
                return PokerPlayerState.none;

            object state = Enum.Parse(typeof(PokerPlayerState), gameState);
            if (state != null)
                return (PokerPlayerState)state;
            else
            {
                Logger.LogWarning("Không thể chuyển đổi từ gameState: '{0}' sang enum PokerPlayerState", gameState);
                return PokerPlayerState.none;
            }
        }

        public double GetGlobalAvailableChip()
        {
            if (globalAsset == null || globalAsset.GetAsset(EAssets.Chip) == null)
                return 0;
            return globalAsset.GetAsset(EAssets.Chip).value;
        }

        internal void SetGlobalInfo(DataUser dataUser)
        {
            _globalInfo = dataUser;
        }
        public DataUser GetUserInfo()
        {
            return _globalInfo;
        }
    }
}
