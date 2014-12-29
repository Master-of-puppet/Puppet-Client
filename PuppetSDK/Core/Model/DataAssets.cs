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

        public void UpdateAssets(DataAssetItem[] updateContents)
        {
            if (updateContents == null) return;
            List<DataAssetItem> listAsset = new List<DataAssetItem>(content);
            for(int i=0;i<updateContents.Length;i++)
            {
                DataAssetItem newContent = updateContents[i];
                DataAssetItem oldContent = listAsset.Find(asset => asset.assetType.name == newContent.assetType.name);
                if (oldContent == null)
                    listAsset.Add(newContent);
                else
                    oldContent.value = newContent.value;
            }
            content = listAsset.ToArray();
        }
    }

    public class DataAssetItem : DataModel
    {
        public DataAssetType assetType { get; set; }
        public double value { get; set; }

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
