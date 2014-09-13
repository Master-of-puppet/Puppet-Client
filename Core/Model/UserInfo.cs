using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class UserInfo : DataModel
    {
        public DataAssets assets { get; set; }
        public DataUser info { get; set; }

        public UserInfo() : base()
        {
        }

        public UserInfo(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
    	{				
   	 	}

        public UserInfo(DataUser info, DataAssets assets)
        {
            this.info = info;
            this.assets = assets;
        }
    }
}
