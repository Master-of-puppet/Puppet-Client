using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataPlayerController : DataModel
    {
        public int id { get; set; }
        public string userName { get; set; }
        public DataAssets asset { get; set; }
        public string gameState { get; set; }
        public string avatar { get; set; }
        public int slotIndex { get; set; }
        public bool isMaster { get; set; }

        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }

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
