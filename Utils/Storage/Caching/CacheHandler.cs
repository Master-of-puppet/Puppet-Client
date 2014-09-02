using System;
using System.Collections.Generic;
using Puppet.Utils.Storage;

namespace Puppet.Utils
{
    public sealed class CacheHandler : BaseSingleton<CacheHandler>, IStorage, IStorageFile
    {
        AbstractCacheService _cache;

        protected override void Init()
        {
            switch (PuMain.Setting.Platform)
            {
                case EPlatform.WebPlayer:
                    _cache = new WebplayerCacheService();
                    break;
                default:
                    _cache = new DeviceCacheService(PuMain.Setting.PathCache);
                    break;
            }

            try
            {
                LoadFile((bool status) => {
                    Logger.Log("Load cache file {0}: {1}", PuMain.Setting.PathCache, status);
                });
            }
            catch (System.IO.FileNotFoundException e)
            {
                Logger.Log("{0}: Cache file {1} not found", e.Message, PuMain.Setting.PathCache);
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }

        public void SetInt(string key, int value)
        {
            _cache.SetInt(key, value);
        }

        public void SetFloat(string key, float value)
        {
            _cache.SetFloat(key, value);
        }

        public void SetString(string key, string value)
        {
            _cache.SetString(key, value);
        }

        public void SetObject(string key, object value)
        {
            _cache.SetObject(key, value);
        }

        public int GetInt(string key)
        {
            return _cache.GetInt(key);
        }

        public float GetFloat(string key)
        {
            return _cache.GetFloat(key);
        }

        public string GetString(string key)
        {
            return _cache.GetString(key);
        }

        public object GetObject(string key)
        {
            return _cache.GetObject(key);
        }

        public void DeleteAll()
        {
            _cache.DeleteAll();
        }

        public void DeleteKey(string key)
        {
            _cache.DeleteKey(key);
        }

        public bool HasKey(string key)
        {
            return _cache.HasKey(key);
        }

        public void SaveFile(Action<bool> callback)
        {
            _cache.SaveFile(callback);
        }

        public void LoadFile(Action<bool> callback)
        {
            _cache.LoadFile(callback);
        }

        public void DeleteFile(Action<bool> callback)
        {
            _cache.DeleteFile(callback);
        }

        public string GetFullKey(string key)
        {
            return _cache.GetFullKey(key);
        }
    }
}
