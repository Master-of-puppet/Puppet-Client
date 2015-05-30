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
        public AppConfigDetail [] items { get; set; }

        public ResponseAppConfig() : base() { }

        public string GetValue(string key)
        {
            if(items != null && items.Length > 0)
                return System.Array.Find<AppConfigDetail>(items, config => config.name == key).value;

            return string.Empty;
        }
    }

    public class AppConfigDetail : DataModel
    {
        public string name { get; set; }
        public string value { get; set; }

        public AppConfigDetail() : base() { }
    }
}
