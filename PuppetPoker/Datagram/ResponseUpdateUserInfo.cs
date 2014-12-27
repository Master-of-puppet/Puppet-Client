using Puppet.Core.Model;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Poker.Datagram
{
    public class ResponseUpdateUserInfo : DataModel
    {
        public string userName { get; set; }
        public string field { get; set; }
        public string command { get; set; }
        public DataAssets asset { get; set; }

        public ResponseUpdateUserInfo()
            : base()
        {
        }
    }
}
