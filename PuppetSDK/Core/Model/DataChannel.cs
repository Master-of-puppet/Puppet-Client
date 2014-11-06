using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataChannel : RoomInfo
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public int childCount { get; set; }
		public DataChannelConfiguration configuration { get; set; }

        public DataChannel() 
            : base()
        {
        }

        public DataChannel(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
    	{				
   	 	}
    }

	public class DataChannelConfiguration : DataModel
	{
        public double[] betting { get; set; }

		public DataChannelConfiguration() 
			: base()
		{
		}
	}
}
