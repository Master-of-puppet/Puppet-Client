using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataPlaza : DataModel
    {
        public DataPlaza() 
            : base()
        {
        }

        public DataPlaza(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
    	{				
   	 	}
    }
}
