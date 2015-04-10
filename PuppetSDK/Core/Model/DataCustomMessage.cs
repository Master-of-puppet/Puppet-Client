using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataCustomMessage : DataModel
    {
        public string command { get; set; }
        public string content { get; set; }

        public DataCustomMessage() : base() { }
    }
}
