using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Puppet.Core.Model
{
    public class DataUser : DataModel
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string avatar { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        
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
