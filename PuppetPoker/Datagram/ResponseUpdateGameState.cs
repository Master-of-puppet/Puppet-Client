using Puppet.Core.Model;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Poker.Datagram
{
    public class ResponseUpdateGameState : DataModel
    {
        public string gameState { get; set; }
		public string command { get; set; }
		public string lastGameState { get; set; }

        public ResponseUpdateGameState()
            : base()
        {
        }

        public PokerGameState GetCurrentState()
        {
            object state = Enum.Parse(typeof(PokerGameState), gameState);
            if (state != null)
                return (PokerGameState)state;
            else
            {
                Logger.LogWarning("Không thể chuyển đổi từ gameState: '{0}' sang enum PokerGameState", gameState);
                return PokerGameState.none;
            }
        }

        public PokerGameState GetLastState()
        {
            object state = Enum.Parse(typeof(PokerGameState), lastGameState);
            if (state != null)
                return (PokerGameState)state;
            else
            {
                Logger.LogWarning("Không thể chuyển đổi từ lastGameState: '{0}' sang enum PokerGameState", lastGameState);
                return PokerGameState.none;
            }
        }
    }
}
