using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model.Datagram
{
    internal class ResponseListLobby : DataModel
    {
        public string command { get; set; }
        public DataLobby[] children { get; set; }

        public ResponseListLobby()
            : base()
        {
        }

        public ResponseListLobby(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }
    }
}
