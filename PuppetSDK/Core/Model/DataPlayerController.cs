using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataPlayerController : DataModel
    {
        public string userName { get; set; }
        public DataGameAssets asset { get; set; }
        public string gameState { get; set; }
        public int slotIndex { get; set; }
        public bool isMaster { get; set; }

        public DataPlayerController() 
            : base()
        {
        }

        public DataPlayerController(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
    	{				
   	 	}
    }
}
