using Puppet.Core.Model;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Poker.Datagram
{
    public class ResponseUpdateRoomMaster : DataModel
    {
		public string command { get; set; }
		public PokerPlayerController player { get; set; }

        public ResponseUpdateRoomMaster()
            : base()
        {
        }
    }
}
