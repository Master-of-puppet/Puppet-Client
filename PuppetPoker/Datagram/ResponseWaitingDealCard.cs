using Puppet.Core.Model;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Poker.Datagram
{
    public class ResponseWaitingDealCard : DataModel
    {
        public string command { get; set; }
        public int time { get; set; }

        public ResponseWaitingDealCard()
            : base()
        {
        }
    }
}
