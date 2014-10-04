using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model.Datagram
{
    public class RequestPlugin : AbstractData
    {
        public const string NODE_PLUGIN_VALUE = "NodePlugin";
        public const string GAME_PLUGIN_VALUE = "GamePlugin";
        public const string OBSERVER_PLUGIN_VALUE = "ObserverPlugin";

        public string pluginName {get;set;}
        public AbstractData data {get;set;}

        public RequestPlugin(AbstractData data) : this(data, NODE_PLUGIN_VALUE) { }
        public RequestPlugin(AbstractData data, string pluginName)
        {
            this.data = data;
            this.pluginName = pluginName;
        }
    }
}
