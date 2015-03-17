using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataResponseRecharge : DataModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public string support_phone { get; set; }

        public DataRecharge[] sms { get; set; }
        public DataRecharge[] card { get; set; }

        public DataResponseRecharge() : base() { }
    }

    public class DataRecharge : DataModel
    {
        public string type { get; set; }
        public string code { get; set; }
        public string code_value { get; set; }
        public string provider { get; set; }
        public string template { get; set; }
        public string image { get; set; }

        public DataRecharge() : base() { }
    }
}
