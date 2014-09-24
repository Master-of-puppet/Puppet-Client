using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataGameplay : DataModel
    {
        public string GameState { get; set; }
        public DataPlayerController[] Players {get;set; }
        public int IndexInTurn { get; set; }
        
        public DataGameplay() 
            : base()
        {
        }

        public DataGameplay(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
    	{				
   	 	}
    }
}
