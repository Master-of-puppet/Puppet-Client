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
        public int betting { get; set; }

        public DataConfigGame()
            : base()
        {
        }

        public DataConfigGame(int betting)
        {
            this.betting = betting;
        }
    }
}
