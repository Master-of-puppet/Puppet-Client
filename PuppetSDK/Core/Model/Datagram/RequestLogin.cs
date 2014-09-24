using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model.Datagram
{
    internal class RequestLogin : AbstractData
    {
        public string token { get; set; }
        public DataClientDetails clientDetails { get; set; }

        internal RequestLogin(string token)
        {
            this.token = token;
            this.clientDetails = PuMain.Setting.ClientDetails;
        }
    }
    
}
