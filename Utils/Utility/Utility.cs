using Puppet.Core.Model;
using Puppet.Core.Model.Factory;
using Puppet.Core.Network.Socket;
using Sfs2X.Entities.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Puppet.Utils
{
    public class Utility
    {
        public static void StartCoroutine(IEnumerator ienumerator)
        {
            try { while (!ienumerator.MoveNext()) break; }
            finally
            {
                IDisposable disposable = ienumerator as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
        }

        public static string SFSObjectToString(ISFSObject obj)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (string key in obj.GetKeys())
                dict.Add(key, obj.GetData(key).Data);
            return JsonUtil.Serialize(dict);
        }

        public static FieldInfo[] GetFieldInfo(Type t, BindingFlags bindingFlags)
        {
            return t.GetFields(bindingFlags);
        }

        public static T GetDataFromResponse<T>(ISocketResponse response, string key) where T : AbstractData
        {
            return SFSDataModelFactory.CreateDataModel<T>((SFSObject)response.Params[key]);
        }

        public static T GetDataFromResponse<T>(ISocketResponse response, string parentKey, string key) where T : AbstractData
        {
            SFSObject obj = (SFSObject)response.Params[parentKey];
            return SFSDataModelFactory.CreateDataModel<T>((SFSObject)obj.GetSFSObject(key));
        }

        public static T GetCustomAttribute<T>(Enum e) where T : System.Attribute
        {
            return GetCustomAttributes<T>(e).FirstOrDefault();
        }

        public static List<T> GetCustomAttributes<T>(Enum e) where T : System.Attribute
        {
            List<T> attrs = new List<T>();
            System.Reflection.FieldInfo fi = e.GetType().GetField(e.ToString());
            object[] objs = fi.GetCustomAttributes(typeof(T), false);
            foreach (object a in objs)
                if (a is T) attrs.Add((T)a);
            return attrs;
        }

    }
}
