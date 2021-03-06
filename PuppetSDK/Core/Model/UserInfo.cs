﻿using System;
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

        public void UpdateInfo(DataUser newInfo, DataAssets newAssets)
        {
            if (info != null)
            {
                this.info.UpdateData(newInfo);
                DispatchAttribute("info", false);
            }

            if (newAssets != null && newAssets.content != null)
            {
                List<DataAssetItem> currentContent = new List<DataAssetItem>(assets.content);
                foreach (DataAssetItem item in newAssets.content)
                {
                    DataAssetItem findItem = currentContent.Find(c => c.assetType.name == item.assetType.name);
                    if (findItem == null)
                        currentContent.Add(item);
                    else
                    {
                        int index = currentContent.IndexOf(findItem);
                        currentContent[index] = item;
                    }
                }
                assets.content = currentContent.ToArray();
                DispatchAttribute("assets", false);
            }

            DispatchDataChanged();
        }
    }
}
