﻿using System;
using System.Collections.Generic;
using LitJson;
using Puppet.Model;

namespace Puppet.Utils
{
    public class JsonDataModelFactory
    {
        public static T CreateDataModel<T>(string json) where T : IDataModel
        {
            return (T)PuJsonMapper.ToObject<T>(json);
        }

        public static T CreateDataModel<T>(Dictionary<string, System.Object> dict) where T : IDataModel
        {
            return (T)JsonDataModelFactory.CreateDataModel<T>(JsonUtil.Serialize(dict));
        }
    }
}

