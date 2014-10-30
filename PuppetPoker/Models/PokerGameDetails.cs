using Puppet.Core.Model;
using Puppet.Core.Model.Datagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Poker.Models
{
    public class PokerGameDetails : DataModel
    {
        public RoomInfo parent { get; set; }
        public DataConfigGame customConfiguration { get; set; }
        public DataChannelConfiguration configuration { get; set; }

        public PokerGameDetails() 
            : base()
        {
        }
    }
}
