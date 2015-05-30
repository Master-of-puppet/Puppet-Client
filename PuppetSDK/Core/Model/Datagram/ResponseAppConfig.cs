using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Model
{
    public class ResponseAppConfig : DataModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public AppConfigDetail items { get; set; }

        public ResponseAppConfig() : base() { }
    }

    public class AppConfigDetail : DataModel
    {
        public string name { get; set; }
        public string value { get; set; }

        public AppConfigDetail() : base() { }
    }
}
