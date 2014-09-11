using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model.Datagram
{
    internal class RequestGetGroupName : AbstractData
    {
        public string command { get; set; }
        public string groupName { get; set; }

        internal RequestGetGroupName(string command, string groupName)
        {
            this.command = command;
            this.groupName = groupName;
        }
    }
    
}
