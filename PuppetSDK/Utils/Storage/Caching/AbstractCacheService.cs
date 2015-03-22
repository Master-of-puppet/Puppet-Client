using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Puppet.Utils.Storage
{
    abstract class AbstractCacheService : IStorage, IStorageFile
	{
        #region Cache Model
        [Serializable()]
        internal class CacheModel : DataModel
        {
            public string type;
            public string value;
            public CacheModel() : base() { }
            public CacheModel(string type, string value)
            {
                this.type = type;
                this.value = value;
            }
            public CacheModel(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }
        }
        #endregion

        private delegate bool delegateRunAsync();

        protected const string KEY_VALUE_INT = "INT";
        protected const string KEY_VALUE_STRING = "STRING";
        protected const string KEY_VALUE_FLOAT = "FLOAT";
        protected const string KEY_VALUE_OBJECT = "OBJECT";

        protected const string DUPLICATE_KEY_FOR_VALUE_TYPES_EXCEPTION = "Key has already been used for other value types";
        protected const string KEY_NOT_FOUND = "Key not found";

        public Dictionary<string, CacheModel> memCache = new Dictionary<string, CacheModel>();

        public Dictionary<string, string> MemCacheToString()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach(string key in memCache.Keys)
                dict.Add(key, memCache[key].ToJson());
            return dict;
        }

        private void SetKeyValue(string fullKey, string value, string type)
        {
            if (HasKey(fullKey))
            {
                CacheModel tmp = memCache[fullKey];
                if (tmp.type != type)
                    throw new ArgumentException(DUPLICATE_KEY_FOR_VALUE_TYPES_EXCEPTION);
                else
                    memCache[fullKey] = new CacheModel(type, value);
            }
            else
            {
                memCache.Add(fullKey, new CacheModel(type, value));
            }
        }

        private void RunAync(delegateRunAsync action, Action<bool> callback)
        {
            ThreadHandler.RunAsync(() =>
            {
                bool result = false;
                try
                {
                    result = action();
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
                finally
                {
                    if (callback != null)
                        callback(result);
                }
            });
        }

        public void SetInt(string key, int value)
        {
            key = GetFullKey(key);
            SetKeyValue(key, value.ToString(), KEY_VALUE_INT);
        }

        public void SetFloat(string key, float value)
        {
            key = GetFullKey(key);
            SetKeyValue(key, value.ToString(), KEY_VALUE_FLOAT);
        }

        public void SetString(string key, string value)
        {
            key = GetFullKey(key);
            SetKeyValue(key, value, KEY_VALUE_STRING);
        }

        public virtual void SetObject(string key, object value)
        {
            key = GetFullKey(key);
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, value);
                SetKeyValue(key, Convert.ToBase64String(ms.ToArray()), KEY_VALUE_OBJECT);
            }
        }

        public int GetInt(string key)
        {
            key = GetFullKey(key);
            if (HasKey(key))
            {
                CacheModel model = memCache[key];
                if (model.type == KEY_VALUE_INT)
                    return Convert.ToInt32(model.value);
                else
                    throw new ArgumentException(KEY_NOT_FOUND);
            }
            else
                throw new ArgumentException(KEY_NOT_FOUND);
        }

        public float GetFloat(string key)
        {
            key = GetFullKey(key);
            if (HasKey(key))
            {
                CacheModel model = memCache[key];
                if (model.type == KEY_VALUE_FLOAT)
                    return (float)Convert.ToDouble(model.value);
                else
                    throw new ArgumentException(KEY_NOT_FOUND);
            }
            else
                throw new ArgumentException(KEY_NOT_FOUND);
        }

        public string GetString(string key)
        {
            key = GetFullKey(key);
            if (HasKey(key))
            {
                CacheModel model = memCache[key];
                if (model.type == KEY_VALUE_STRING)
                    return model.value;
                else
                    throw new ArgumentException(KEY_NOT_FOUND);
            }
            else
                throw new ArgumentException(KEY_NOT_FOUND);
        }

        public virtual object GetObject(string key)
        {
            key = GetFullKey(key);
            if (HasKey(key))
            {
                CacheModel model = memCache[key];

                if (model.type == KEY_VALUE_OBJECT)
                {
                    byte[] bytes = Convert.FromBase64String(model.value);
                    using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
                    {
                        ms.Write(bytes, 0, bytes.Length);
                        ms.Position = 0;
                        return new BinaryFormatter().Deserialize(ms);
                    }
                }
                else
                {
                    throw new Exception("Type invalid: " + model.type + " - not is: " + KEY_VALUE_OBJECT);
                }
            }
            else
            {
                throw new ArgumentException(KEY_NOT_FOUND);
            }
        }

        public void DeleteAll()
        {
            memCache = new Dictionary<string, CacheModel>();
        }

        public void DeleteKey(string key)
        {
            key = GetFullKey(key);
            if (HasKey(key))
                memCache.Remove(key);
            else
                throw new KeyNotFoundException("");
        }

        public bool HasKey(string key)
        {
            return memCache.ContainsKey(key);
        }

        public bool HasFullKey(string key)
        {
            return memCache.ContainsKey(GetFullKey(key));
        }

        public string GetFullKey(string key)
        {
            return string.Format("Caching_{0}", key);
        }

        public void SaveFile(Action<bool> callback)
        {
            RunAync(SaveSync, callback);
        }

        public void LoadFile(Action<bool> callback)
        {
            RunAync(LoadSync, callback);
        }

        public void DeleteFile(Action<bool> callback)
        {
            RunAync(DeleteFileSync, callback);
        }

        protected abstract bool LoadSync();

        protected abstract bool SaveSync();

        protected abstract bool DeleteFileSync();
    }
}
