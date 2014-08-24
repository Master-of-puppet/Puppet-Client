using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puppet.Utils
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Gets or add a component. Usage example:
        /// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
        /// </summary>
        static public T GetOrAddComponent<T>(this Component child) where T : Component
        {
            T result = child.GetComponent<T>();
            if (result == null)
                result = child.gameObject.AddComponent<T>();

            return result;
        }

        /// <summary>
        /// Gets or add a component. Usage example:
        /// BoxCollider boxCollider = gameObject.GetOrAddComponent<BoxCollider>();
        /// </summary>
        static public T GetOrAddComponent<T>(this GameObject obj) where T : MonoBehaviour
        {
            T result = obj.GetComponent<T>();
            if (result == null)
                result = obj.gameObject.AddComponent<T>();

            return result;
        }

    }
}
