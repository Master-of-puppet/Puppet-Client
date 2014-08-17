using System;
using System.Collections.Generic;

namespace Puppet.Patterns
{
    public abstract class BaseSingleton<T> where T : BaseSingleton<T>, new()
    {
        static T _instance;
        public static T Instance
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = new T();
                    _instance.Init();
                }
                return _instance;
            }
        }

        public abstract void Init();
    }
}
