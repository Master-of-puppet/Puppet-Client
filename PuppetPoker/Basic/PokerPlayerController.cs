using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Puppet.Poker
{
    public class PokerPlayerController : DataPlayerController
    {
        public List<PokerCard> cardsOnHand { get; set; }
        public int slotServer { get; set; }
        public int playerState { get; set; }
        public int handSize { get; set; }

        public PokerPlayerController() 
            : base()
        {
        }

        public PokerPlayerController(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
    	{				
   	 	}
    }
}
