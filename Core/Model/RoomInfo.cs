using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class RoomInfo : DataModel
    {
        public int roomId { get; set; }
        public int zoneId { get; set; }

        public RoomInfo() : base()
        {
        }

        public RoomInfo(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
    	{				
   	 	}

        public RoomInfo(int roomId)
        {
            this.roomId = roomId;
        }
    }
}
