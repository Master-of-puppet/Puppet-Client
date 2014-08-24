using System;
using System.Collections.Generic;

namespace Puppet
{
    public abstract class Singleton<T> where T : UnityEngine.MonoBehaviour
    {
        private static T _instance;
        private static object _lock = new object();

        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Logger.LogWarning("[Singleton] Instance '{0}' already destroyed on application quit. Won't create again - returning null.", typeof(T));
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)UnityEngine.GameObject.FindObjectOfType(typeof(T));

                        if (UnityEngine.GameObject.FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Logger.LogError("[Singleton] Something went really wrong - there should never be more than 1 singleton! Reopenning the scene might fix it.");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            UnityEngine.GameObject singleton = new UnityEngine.GameObject();
                            _instance = singleton.AddComponent<T>();
                            singleton.name = "(Singleton) " + typeof(T).ToString();

                            UnityEngine.GameObject.DontDestroyOnLoad(singleton);
                            Logger.Log("[Singleton] An instance of {0} is needed in the scene, so '{1}' was created with DontDestroyOnLoad.", typeof(T), singleton);
                        }
                        else
                            Logger.Log("[Singleton] Using instance already created: {0}", _instance.gameObject.name);
                    }

                    return _instance;
                }
            }
        }

        private static bool applicationIsQuitting = false;
        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        ///   it will create a buggy ghost object that will stay on the Editor scene
        ///   even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        public virtual void OnDestroy()
        {
            applicationIsQuitting = true;
        }
    }
}
