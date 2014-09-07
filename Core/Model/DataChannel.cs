﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataChannel : DataModel
    {
        public int roomId { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public string roomName { get; set; }
        public string displayName { get; set; }
        public string groupName { get; set; }

        public DataChannel() 
            : base()
        {
        }

        public DataChannel(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
    	{				
   	 	}
    }
}