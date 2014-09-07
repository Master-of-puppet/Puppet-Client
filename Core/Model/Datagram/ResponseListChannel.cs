using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model.Datagram
{
    internal class ResponseListChannel : DataModel
    {
        public string command { get; set; }
        public DataChannel[] children { get; set; }

        public ResponseListChannel()
            : base()
        {
        }

        public ResponseListChannel(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }
    }
}
