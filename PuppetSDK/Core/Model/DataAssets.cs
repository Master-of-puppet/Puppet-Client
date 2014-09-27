using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataAssets : DataModel
    {
        public DataAssetItem[] content { get; set; }
        
        public DataAssets() 
            : base() {}

        public DataAssets(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt) {}

        public DataAssetItem GetAsset(EAssets asset)
        {
            return Array.Find<DataAssetItem>(content, item => 
                item.assetType.name == CustomAttribute.GetCustomAttribute<AttributeAsset>(asset).Name
            );
        }
    }

    public class DataAssetItem : DataModel
    {
        public DataAssetType assetType { get; set; }
        public int value { get; set; }

        public DataAssetItem()
            : base() { }

        public DataAssetItem(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt) { }
    }

    public class DataAssetType : DataModel
    {
        public string description { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }

        public DataAssetType()
            : base() {}

        public DataAssetType(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt) {}
    }
}
