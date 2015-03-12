using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model.Datagram
{
    internal class ResponseQuickJoinGame : DataModel
    {
        public string command { get; set; }
        public string message { get; set; }
        public int roomId { get; set; }


        public ResponseQuickJoinGame()
            : base()
        {
        }

        public ResponseQuickJoinGame(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }
    }
}
