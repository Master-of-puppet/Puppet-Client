using Puppet.Poker;
using Puppet.Poker.Datagram;
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


        public static void SitDown(PokerSide side)
        {
            PokerMain.Instance.game.SendSitDown(side);
        }

        public static void PlayRequest(RequestPlay request, long value)
        {
            PokerMain.Instance.game.SendPlayRequest(request, value);
        }
    }
}
