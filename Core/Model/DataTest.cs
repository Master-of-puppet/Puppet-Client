using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Puppet.Core.Model
{
    public class DataTest : DataModel
    {
        public string command { get; set; }
        public string name { get; set; }

        public DataTest() 
            : base()
        {

        }

        public DataTest(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
    	{				
   	 	}
    }
}
