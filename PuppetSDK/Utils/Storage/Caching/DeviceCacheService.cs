using Puppet.Core.Model.Factory;
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
                AOTSafe.SetEnvironmentVariables();
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

                Logger.LogWarning("*** Cache Load JsonData: " + json);

                if (json != null)
                {
                    Dictionary<string, object> jsonDict = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;
                    memCache = new Dictionary<string, CacheModel>();
                    foreach (var item in jsonDict)
                    {
                        CacheModel model = JsonDataModelFactory.CreateDataModel<CacheModel>(item.Value.ToString());
                        //CacheModel model = JsonDataModelFactory.CreateDataModel<CacheModel>(item.Value as Dictionary<string, object>);
                        memCache.Add(item.Key, model);
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

            //string data = MiniJSON.Json.Serialize(memCache);
            string data = MiniJSON.Json.Serialize(MemCacheToString());
            Logger.LogWarning("*** Cache Save JsonData: " + data);

            AOTSafe.SetEnvironmentVariables();
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
