﻿using Puppet.Core.Model;
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
        public static PokerGameplay GetPokerGameplay()
        {
            return PokerMain.Instance.game;
        }

        public static void SitDown(int slotIndex, double money, bool isAutoBuy)
        {
            PuMain.Socket.Request(Puppet.Poker.Datagram.RequestPool.GetSitRequest(PokerRequestPlay.SIT, slotIndex, money));
            PuMain.Socket.Request(RequestPool.SetAutoBuy(isAutoBuy));
        }

        public static void PlayRequest(PokerRequestPlay request, double value)
        {
            PuMain.Socket.Request(Puppet.Poker.Datagram.RequestPool.GetPlayRequest(request, value));
        }

        public static void AutoSitDown()
        {
            PokerMain.Instance.game.AutoSitDown();
        }

        public static void GetOrderHand(Dictionary<string,int[]> cardPlayers)
        {
            PuMain.Socket.Request(RequestPool.GetOrderHand(cardPlayers));
        }

        public static void GetOrderCommunity(int [] cards)
        {
            PuMain.Socket.Request(RequestPool.GetOrderCommunity(cards));
        }

        public static void StandUp()
        {
            PokerMain.Instance.game.StandUp();
        }

        public static void QuitGame()
        {
            PokerMain.Instance.game.QuitGame();
        }

        public static void RequestViewCard()
        {
            Puppet.API.Client.APIGeneric.SendPublicMessage(Constant.KEY_VIEW_CARD, new DataMessageBase());
        }
    }
}
