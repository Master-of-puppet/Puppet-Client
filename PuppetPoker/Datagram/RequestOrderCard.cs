using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Model;

namespace Puppet.Poker.Datagram
{
    internal class RequestOrderCard : AbstractData
    {
        public string command { get; set; }
        public string action { get; set; }
        public RequestPlayerOrderCard [] players { get; set; }
        public int[] cards { get; set; }

        public RequestOrderCard(int [] communityCards)
        {
            this.command = "orderCard";
            this.action = "orderCommunityCard";
            this.cards = communityCards;
        }

        public RequestOrderCard(RequestPlayerOrderCard [] players)
        {
            this.command = "orderCard";
            this.action = "orderHand";
            this.players = players;
        }

        internal class RequestPlayerOrderCard : AbstractData
        {
            public string username { get; set; }
            public int [] cards { get; set; }

            public RequestPlayerOrderCard(string userName, int [] cards)
            {
                this.username = userName;
                this.cards = cards;
            }
        }
    }
}
