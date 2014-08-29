using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataMessage : DataModel
    {
        public int Id { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string Content { get; set; }
        public string TimeSent { get; set; }
        public bool Readed { get; set; }
        public int Status { get; set; }
        public int SenderAvatar { get; set; }
        public int ReceiverAvatar { get; set; }
        public int Type { get; set; }

        public DataMessage() 
            : base()
        {
        }

        public DataMessage(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
    	{				
   	 	}
    }
}
