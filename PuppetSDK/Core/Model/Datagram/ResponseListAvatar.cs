using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Model
{
    public class ResponseListAvatar : DataModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public string [] items { get; set; }

        public ResponseListAvatar() : base() { }
    }
}
