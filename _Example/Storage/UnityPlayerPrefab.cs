using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet
{
    class UnityPlayerPrefab : BaseSingleton<UnityPlayerPrefab>, Utils.Storage.IStorage
    {
        public void SetInt(string key, int value)
        {
            throw new NotImplementedException();
        }

        public void SetFloat(string key, float value)
        {
            throw new NotImplementedException();
        }

        public void SetString(string key, string value)
        {
            throw new NotImplementedException();
        }

        public void SetObject(string key, object value)
        {
            //NotImplement
            throw new NotImplementedException();
        }

        public int GetInt(string key)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(string key)
        {
            throw new NotImplementedException();
        }

        public string GetString(string key)
        {
            throw new NotImplementedException();
        }

        public object GetObject(string key)
        {
            //NotImplement
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public void DeleteKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool HasKey(string key)
        {
            throw new NotImplementedException();
        }

        public string GetFullKey(string key)
        {
            //NotImplement
            throw new NotImplementedException();
        }

        public override void Init()
        {
            throw new NotImplementedException();
        }
    }
}
