using Puppet.Core.Model;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Poker.Datagram
{
    public class ResponseUpdateHand : DataModel
    {
        public PokerPlayerController[] players { get; set; }
        public int[] hand { get; set; }
        public string command { get; set; }
        public int timeForAnimation { get; set; }
        public string dealer { get; set; }

        public ResponseUpdateHand()
            : base()
        {
        }
    }
}
