using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Puppet.Utils
{
    /// <summary>
    /// Author: vietdungvn88@gmail.com
    /// Serialize on iOS , that is AOT safe.
    /// </summary>
    public class AOTSafe
    {
        public static void ForEach<T>(object enumerable, System.Action<T> action)
        {
            if (enumerable == null) return;

            Type listType = enumerable.GetType().GetInterfaces().First(x => !(x.IsGenericType) && x == typeof(IEnumerable));
            if (listType == null)
                throw new ArgumentException("Object does not implement IEnumerable interface", "enumerable");

            MethodInfo method = listType.GetMethod("GetEnumerator");
            if (method == null)
                throw new InvalidOperationException("Failed to get 'GetEnumberator()' method info from IEnumerable type");

            IEnumerator enumerator = null;
            try
            {
                enumerator = (IEnumerator)method.Invoke(enumerable, null);
                if (enumerator is IEnumerator)
                {
                    while (enumerator.MoveNext())
                        action((T)enumerator.Current);
                }
                else
                    Logger.Log(string.Format("{0}.GetEnumerator() returned '{1}' instead of IEnumerator.", enumerable.ToString(), enumerator.GetType().Name));
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
        }

        /// <summary>
        /// Forces a different code path in the BinaryFormatter that doesn't rely on run-time code generation (which would break on iOS)
        /// </summary>
        public static void SetEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
        }
    }
}
