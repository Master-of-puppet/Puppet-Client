using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Puppet.Core.Model
{
    public class DataUser : DataModel
    {
        public int Id { get; set; }
        public string User { get; set; }
        public bool isMe { get; set; }

        public DataUser()
            : base()
        {

        }

        public DataUser(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
    	{				
   	 	}
    }
}
