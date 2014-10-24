﻿using System;
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
        }
    }

    public class DataConfigGame : DataModel
    {
        public DataConfigGame() 
            : base()
        {
        }
    }

    public class DataConfigGamePoker : DataConfigGame
    {
        public int betting { get; set; }

        public DataConfigGamePoker()
            : base()
        {
        }

        public DataConfigGamePoker (int betting)
        {
            this.betting = betting;
        }
    }
    
}
