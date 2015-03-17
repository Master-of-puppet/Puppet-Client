using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataMessageBase : DataModel
    {
        public DataUser Sender { get; set; }
        public long TimeSent { get; set; }

        public DataMessageBase() 
            : base()
        {
            this.Sender = PuGlobal.Instance.mUserInfo.info;
            this.TimeSent = DateTime.Now.Ticks;
        }

        public DataMessageBase(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
    	{				
   	 	}

        public DateTime GetTimeSent()
        {
            return new DateTime(this.TimeSent);
        }
    }
}
