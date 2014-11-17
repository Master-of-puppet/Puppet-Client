using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Model;

namespace Puppet.Poker.Datagram
{
    internal class RequestAutoBuy : AbstractData
    {
        public string command { get; set; }
        public string action { get; set; }
        public bool autoBuy { get; set; }

        public RequestAutoBuy(bool autoBuy)
        {
            this.command = "play";
            this.action = "autoBuy";
            this.autoBuy = autoBuy;
        }
    }
}
