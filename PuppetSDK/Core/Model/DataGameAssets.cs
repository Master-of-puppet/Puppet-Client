using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataGameAssets : DataModel
    {
        public DataGameAssetItem[] content { get; set; }
        
        public DataGameAssets() 
            : base() {}

        public DataGameAssetItem GetAsset(EAssets asset)
        {
            return Array.Find<DataGameAssetItem>(content, item => 
                item.name == CustomAttribute.GetCustomAttribute<AttributeAsset>(asset).Name
            );
        }
    }

    public class DataGameAssetItem : DataModel
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public double value { get; set; }

        public DataGameAssetItem()
            : base() { }
    }
}
