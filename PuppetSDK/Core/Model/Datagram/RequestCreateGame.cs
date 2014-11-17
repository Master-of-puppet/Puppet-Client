using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model.Datagram
{
    internal class RequestCreateGame : AbstractData
    {
        public string command { get; set; }
        public string groupName { get; set; }
        public string gameType { get; set; }
        public DataConfigGame customConfiguration { get; set; }

        internal RequestCreateGame(string command, string groupName, string gameType, DataConfigGame config)
        {
            this.command = command;
            this.groupName = groupName;
            this.gameType = gameType;
            this.customConfiguration = config;
        }
    }

    public class DataConfigGame : DataModel
    {
        public double betting { get; set; }
        public int numPlayers { get; set; }

        public DataConfigGame()
            : base()
        {
        }

        public DataConfigGame(double betting, int numberPlayer)
        {
            this.betting = betting;
            this.numPlayers = numberPlayer;
        }

        public double SmallBlind
        {
            get { return betting / 2f; }
        }
        public double MaxBlind
        {
            get { return betting; }
        }
    }
}
