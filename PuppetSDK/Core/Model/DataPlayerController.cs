using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataPlayerController : DataUser
    {
        public DataAssets asset { get; set; }
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

        public double GetAsset(EAssets type)
        {
            if (asset == null || asset.GetAsset(type) == null)
                return 0;
            return asset.GetAsset(type).value;
        }
        public double GetMoney()
        {
            return GetAsset(EAssets.Chip);
        }
        public double GetExp()
        {
            return GetAsset(EAssets.Experience);
        }
    }
}
