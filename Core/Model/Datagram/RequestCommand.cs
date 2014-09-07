using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model.Datagram
{
    internal class RequestCommand : AbstractData
    {
        public string command { get; set; }

        internal RequestCommand(string command)
        {
            this.command = command;
        }
    }
    
}
