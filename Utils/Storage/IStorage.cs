using System;
using System.Collections.Generic;

namespace Puppet.Utils.Storage
{
    public interface IStorage
    {
        void SetInt(string key, int value);
        void SetFloat(string key, float value);
        void SetString(string key, string value);
        void SetObject(string key, object value);

        int GetInt(string key);
        float GetFloat(string key);
        string GetString(string key);
        object GetObject(string key);

        void DeleteAll();
        void DeleteKey(string key);

        bool HasKey(string key);

        string GetFullKey(string key);
    }

    interface IStorageFile
    {
        void SaveFile(Action<bool> callback);
        void LoadFile(Action<bool> callback);
        void DeleteFile(Action<bool> callback);
    }
}
