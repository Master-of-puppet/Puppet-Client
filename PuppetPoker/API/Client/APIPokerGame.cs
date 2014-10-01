using Puppet.Poker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.API.Client
{
    public static class APIPokerGame
    {
        public static PokerGameplay GetGameplay()
        {
            return PokerMain.Instance.game;
        }
    }
}
