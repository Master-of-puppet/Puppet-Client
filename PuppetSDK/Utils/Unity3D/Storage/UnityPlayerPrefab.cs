using Puppet.Utils.Storage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puppet
{
    internal class UnityPlayerPrefab : BaseSingleton<UnityPlayerPrefab>, IStorage
    {
        public void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public void SetObject(string key, object value)
        {
        }

        public int GetInt(string key)
        {
            return PlayerPrefs.GetInt(key);
        }

        public float GetFloat(string key)
        {
            return PlayerPrefs.GetFloat(key);
        }

        public string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public object GetObject(string key)
        {
            return null;
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        public void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public string GetFullKey(string key)
        {
            return null;
        }

        protected override void Init()
        {
        }
    }
}
