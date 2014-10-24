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
        public static void StartListenerEvent()
        {
            PokerMain.Instance.game.StartListenerEvent();
        }

        public static PokerGameplay GetPokerGameplay()
        {
            return PokerMain.Instance.game;
        }

        public static void SitDown(int slotIndex)
        {
            PokerMain.Instance.game.SendSitDown(slotIndex);
        }

        public static void PlayRequest(PokerRequestPlay request, long value)
        {
            PokerMain.Instance.game.SendPlayRequest(request, value);
        }

        public static void AutoSitDown()
        {
            PokerMain.Instance.game.AutoSitDown();
        }
    }
}
