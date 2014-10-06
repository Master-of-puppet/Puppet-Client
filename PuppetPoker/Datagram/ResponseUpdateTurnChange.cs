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
        public string command { get; set; }
        public string action { get; set; }

        public ResponseUpdateTurnChange()
            : base()
        {
        }

        public PokerGameState GetGameState()
        {
            object state = Enum.Parse(typeof(PokerGameState), command);
            if (state != null)
                return (PokerGameState)state;
            else
            {
                Logger.LogWarning("Không thể chuyển đổi từ gameState: '{0}' sang enum PokerGameState", command);
                return PokerGameState.none;
            }
        }
    }
}
