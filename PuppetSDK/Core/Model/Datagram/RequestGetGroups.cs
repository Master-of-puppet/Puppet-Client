using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model.Datagram
{
    internal class RequestGetGroups : AbstractData
    {
        public string command { get; set; }
        public string groupName { get; set; }
        public string gameType { get; set; }

        internal RequestGetGroups(string command, string groupName)
        {
            this.command = command;
            this.groupName = groupName;
        }

        internal RequestGetGroups(string command, string groupName, string gameType)
            : this(command, groupName)
        {
            this.gameType = gameType;
        }
    }
    
}
