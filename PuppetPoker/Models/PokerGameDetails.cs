using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Poker.Models
{
    public class PokerGameDetails : DataModel
    {
        public RoomInfo parent { get; set; }

        public PokerGameDetails() 
            : base()
        {
        }
    }
}
