using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataResponseRecharge : DataModel
    {
        public DataRecharge[] items { get; set; }

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
