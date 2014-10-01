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
    }
}
