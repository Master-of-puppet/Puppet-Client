using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model.Datagram
{
    internal class RequestNodePlugin : AbstractData
    {
        const string NODE_PLUGIN_VALUE = "NodePlugin";

        public string pluginName {get;set;}
        public AbstractData data {get;set;}

        internal RequestNodePlugin(AbstractData data)
        {
            this.pluginName = NODE_PLUGIN_VALUE;
            this.data = data;
        }
    }
    
}
