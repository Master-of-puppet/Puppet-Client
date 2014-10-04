using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model.Datagram
{
    internal class RequestGetGift : AbstractData
    {
        public string token { get; set; }
        public string command { get; set; }

        internal RequestGetGift(DataDailyGift data)
        {
            this.token = data.token;
            this.command = data.commandDesign ?? "getDailyGift";
        }
    }
    
}
