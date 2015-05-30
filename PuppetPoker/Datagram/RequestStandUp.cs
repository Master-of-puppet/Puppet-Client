using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Model;

namespace Puppet.Poker.Datagram
{
    internal class RequestWithAction : AbstractData
    {
        public string command { get; set; }
        public string action { get; set; }

        public RequestWithAction(string action)
        {
            this.command = "play";
            this.action = action;
        }
    }
}
