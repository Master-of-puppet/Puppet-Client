﻿using Puppet.Poker;
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

        public static void SitDown(int slotIndex, double money)
        {
            PokerMain.Instance.game.SendSitDown(slotIndex, money);
        }

        public static void PlayRequest(PokerRequestPlay request, double value)
        {
            PokerMain.Instance.game.SendPlayRequest(request, value);
        }

        public static void AutoSitDown(double money)
        {
            PokerMain.Instance.game.AutoSitDown(money);
        }

        public static void GetOrderHand(Dictionary<string,int[]> cardPlayers)
        {
            PuMain.Socket.Request(RequestPool.GetOrderHand(cardPlayers));
        }

        public static void GetOrderCommunity(int [] cards)
        {
            PuMain.Socket.Request(RequestPool.GetOrderCommunity(cards));
        }

        public static void SetAutoBuy(bool autoBuy)
        {
            PuMain.Socket.Request(RequestPool.SetAutoBuy(autoBuy));
        }

        public static void StandUp()
        {
            PuMain.Socket.Request(RequestPool.GetStandUpRequest());
        }
    }
}
