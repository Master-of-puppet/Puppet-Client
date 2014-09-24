using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataRoomInfo : DataModel
    {
        public int roomId { get; set; }
        public int zoneId { get; set; }

        public DataRoomInfo() 
            : base()
        {
        }

        public DataRoomInfo(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
    	{				
   	 	}
    }
}
