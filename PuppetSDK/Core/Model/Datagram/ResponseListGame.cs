using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model.Datagram
{
    internal class ResponseListGame : DataModel
    {
        public string command { get; set; }
        public DataGame[] children { get; set; }

        public ResponseListGame()
            : base()
        {
        }

        public ResponseListGame(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }
    }
}
