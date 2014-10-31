using Puppet.Core.Model;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Poker.Datagram
{
    public class ResponseUpdateTurnChange : DataModel
    {
        public PokerPlayerController toPlayer { get; set; }
        public PokerPlayerController fromPlayer { get; set; }
        public string command { get; set; }
        public string action { get; set; }
        public int[] dealComminityCards { get; set; }
        public int time { get; set; }
        public bool newRound { get; set; }
        public bool firstTurn { get; set; }
        public int value { get; set; }
        public PokerPlayerController bigBlind { get; set; }
        public PokerPlayerController smallBlind { get; set; }

        public ResponseUpdateTurnChange()
            : base()
        {
        }

        public PokerPlayerState GetActionState()
        {
            object state = Enum.Parse(typeof(PokerPlayerState), action);
            if (state != null)
                return (PokerPlayerState)state;
            else
            {
                Logger.LogWarning("Không thể chuyển đổi từ action: '{0}' sang enum PokerGameState", action);
                return PokerPlayerState.none;
            }
        }
    }
}
