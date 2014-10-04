using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Model;

namespace Puppet.Poker.Datagram
{
    internal class RequestGameAction : AbstractData
    {
        public string command { get; set; }
        public string action { get; set; }
        public long value { get; set; }
        public int slot { get; set; }

        public RequestGameAction(string actionName)
        {
            this.command = "play";
            this.action = actionName;
        }

        public RequestGameAction(string actionName, long value) 
            : this(actionName)
        {
            this.value = value;
        }

        public RequestGameAction(string actionName, int slot)
            : this(actionName)
        {
            this.slot = slot;
        }
    }
}
