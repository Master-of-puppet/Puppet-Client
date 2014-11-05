using Puppet.Core.Network.Socket;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using Puppet.Core.Model.Datagram;

namespace Puppet.Poker.Datagram
{
    internal static class RequestPool
    {
        public static ISocketRequest GetSitRequest(PokerRequestPlay request, int slotIndex, long money)
        {
            ISFSObject obj = new RequestPlugin(new RequestGameAction(request.ToString().ToLower(), slotIndex, money), RequestPlugin.GAME_PLUGIN_VALUE).ToSFSObject();
            return new SFSocketRequest(new ExtensionRequest(Fields.REQUEST_PLUGIN, obj, PuMain.Instance.SfsRoom));
        }

        public static ISocketRequest GetPlayRequest(PokerRequestPlay request, long value)
        {
            ISFSObject obj = new RequestPlugin(new RequestGameAction(request.ToString().ToLower(), value), RequestPlugin.GAME_PLUGIN_VALUE).ToSFSObject();
            return new SFSocketRequest(new ExtensionRequest(Fields.REQUEST_PLUGIN, obj, PuMain.Instance.SfsRoom));
        }

        public static ISocketRequest GetOrderHand(Dictionary<string,int[]> cardPlayers)
        {
            List<RequestOrderCard.RequestPlayerOrderCard> players = new List<RequestOrderCard.RequestPlayerOrderCard>();
            foreach(string key in cardPlayers.Keys)
                players.Add(new RequestOrderCard.RequestPlayerOrderCard(key, cardPlayers[key]));

            ISFSObject obj = new RequestPlugin(new RequestOrderCard(players.ToArray()), RequestPlugin.GAME_PLUGIN_VALUE).ToSFSObject();
            return new SFSocketRequest(new ExtensionRequest(Fields.REQUEST_PLUGIN, obj, PuMain.Instance.SfsRoom));
        }
        public static ISocketRequest GetOrderCommunity(int [] cards)
        {
            ISFSObject obj = new RequestPlugin(new RequestOrderCard(cards), RequestPlugin.GAME_PLUGIN_VALUE).ToSFSObject();
            return new SFSocketRequest(new ExtensionRequest(Fields.REQUEST_PLUGIN, obj, PuMain.Instance.SfsRoom));
        }
    }
}
