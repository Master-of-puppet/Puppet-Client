using Puppet.Core.Model;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Poker.Datagram
{
    public class ResponseUpdatePot : DataModel
    {
        public string command { get; set; }
        public DataPot[] pot { get; set; }

        public ResponseUpdatePot()
            : base()
        {
        }

        public class DataPot : DataModel
        {
            int id;
            int value;
            public DataPot() : base() { }
        }
    }
}
