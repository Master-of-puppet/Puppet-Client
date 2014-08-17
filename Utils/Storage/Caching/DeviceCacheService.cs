using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Puppet.Utils.Storage
{
    sealed class DeviceCacheService : AbstractCacheService
    {
        string filePath = string.Empty;
        internal DeviceCacheService(string savePath)
        {
            filePath = savePath;
        }

        protected override bool LoadSync()
        {
            bool success = false;
            string json = string.Empty;
            if (File.Exists(filePath))
            {
                Stream stream = File.Open(filePath, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Binder = new VersionDeserializationBinder();
                try
                {
                    json = (string)bformatter.Deserialize(stream);
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
                finally
                {
                    stream.Close();
                }
                if (json != null)
                {
                    Dictionary<string, object> jsonDict = JsonUtil.Deserialize(json) as Dictionary<string, object>;
                    memCache = new Dictionary<string, CacheModel>();
                    foreach (var item in jsonDict)
                    {
                        CacheModel model = JsonDataModelFactory.CreateDataModel<CacheModel>(item.Value as Dictionary<string, object>);
                        memCache.Add(item.Key, model);
                        Logger.Log("Key: " + item.Key);
                        Logger.Log("Value: " + model.value);
                    }
                    success = true;
                }
            }
            return success;
        }

        protected override bool SaveSync()
        {
            if (memCache == null)
                throw new ArgumentNullException("data cannot be null.");
            string data = JsonUtil.Serialize(memCache);
            Logger.Log("data: " + data);

            Stream stream = File.Open(filePath, FileMode.OpenOrCreate);
            BinaryFormatter bformatter = new BinaryFormatter();
            bformatter.Binder = new VersionDeserializationBinder();
            bformatter.Serialize(stream, data);
            stream.Close();
            return true;
        }

        protected override bool DeleteFileSync()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            else
                throw new FileNotFoundException();
        }

        private sealed class VersionDeserializationBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                if (!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName))
                {
                    Type typeToDeseralize = null;
                    assemblyName = Assembly.GetExecutingAssembly().FullName;
                    typeToDeseralize = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));
                    return typeToDeseralize;
                }
                return null;
            }
        }
    }
}
