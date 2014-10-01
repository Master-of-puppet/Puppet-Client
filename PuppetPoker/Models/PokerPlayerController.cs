using Puppet.Core.Model;
using System;
using System.Collections.Generic;

namespace Puppet.Poker.Models
{
    public class PokerPlayerController : DataPlayerController
    {
        public PokerPlayerController() 
            : base()
        {
        }

        public PokerSide GetSide()
        {
            return PokerMain.Instance.game.GetSide(this);
        }
    }
}
