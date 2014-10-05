using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataDailyGift : DataModel
    {
        public double[] values { get; set; }
        public string token { get; set; }
        public string command { get; set; }
        public int chainIndex { get; set; }
        public string listenerPlugin { get; set; }
        public string desireCommand { get; set; }

        public DataDailyGift() : base() {}
    }
}
