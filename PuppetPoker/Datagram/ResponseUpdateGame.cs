using Puppet.Core.Model;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Poker.Datagram
{
    public class ResponseUpdateGame : DataModel
    {
        public PokerGameDetails gameDetails { get; set; }
        public string gameState { get; set; }
        public PokerPlayerController[] waitingPlayers { get; set; }
        public string command { get; set; }
        public PokerPlayerController[] players { get; set; }
        public int remainingTime { get; set; }
        public int totalTime { get; set; }
        public int[] dealComminityCards { get; set; }

        public ResponseUpdateGame()
            : base()
        {
        }
    }
}
