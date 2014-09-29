using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataCard : DataModel
    {
        public int cardId { get; set; }
        public int slotId { get; set; }
        
        public DataCard() 
            : base()
        {
        }

        public DataCard(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
    	{				
   	 	}

        public virtual int CompareTo(DataCard card)
        {
            return cardId - card.cardId;
        }
    }

}
