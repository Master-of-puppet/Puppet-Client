using System;
using System.Collections.Generic;
using System.Linq;
using Puppet;
using Puppet.Core.Model;
using System.Runtime.Serialization;

namespace Puppet.Poker.Models
{
    public class PokerCard : DataCard
    {
        public PokerCard(int id)
            : base()
        {
            cardId = id;
        }

        public PokerCard() 
            : base()
        {
            cardId = -1;
        }

        public ECardRank GetRank()
        {
            return (ECardRank)(int)Math.Ceiling((cardId + 1) / 4f);
        }

        public ECardSuit GetSuit()
        {
            return (ECardSuit)(cardId % 4);
        }

        public bool IsRedCard()
        {
            return cardId % 4 > 1;
        }
    }
}
