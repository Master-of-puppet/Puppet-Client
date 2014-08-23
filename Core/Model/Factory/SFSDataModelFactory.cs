using Puppet.Utils;
using Sfs2X.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Model.Factory
{
    public class SFSDataModelFactory
    {
        public static T CreateDataModel<T>(string json) where T : IDataModel
        {
            return (T)PuJsonMapper.ToObject<T>(json);
        }

        public static T CreateDataModel<T>(Dictionary<string, System.Object> dict) where T : IDataModel
        {
            return (T)SFSDataModelFactory.CreateDataModel<T>(JsonUtil.Serialize(dict));
        }

        public static T CreateDataModel<T>(SFSObject obj) where T : IDataModel
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (string key in obj.GetKeys())
                dict.Add(key, obj.GetData(key).Data);
            return (T)SFSDataModelFactory.CreateDataModel<T>(JsonUtil.Serialize(dict));
        }

    }
}
