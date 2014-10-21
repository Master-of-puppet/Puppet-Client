using Puppet.Core.Model;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Poker.Datagram
{
    public class ResponseError : DataModel
    {
        public bool showPopup { get; set; }
        public string command { get; set; }
        public int errorId { get; set; }
        public string errorCode { get; set; }
        public string errorMessage { get; set; }

        public ResponseError()
            : base()
        {
        }
    }
}
