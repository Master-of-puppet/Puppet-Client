using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataBuddy : RoomInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool isBlocked { get; set; }
        public bool isOnline { get; set; }
        public string nickName { get; set; }
        public string state { get; set; }

        public DataBuddy() 
            : base()
        {
        }

        public DataBuddy(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
    	{				
   	 	}

        public DataBuddy(int id, string name) 
        {
            this.id = id;
            this.name = name;
        }
    }
}
