using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet
{
    public class CustomAttribute
    {
        public static T GetCustomAttribute<T>(Enum e) where T : System.Attribute
        {
            return GetCustomsAttribute<T>(e).FirstOrDefault();
        }

        public static List<T> GetCustomsAttribute<T>(Enum e) where T : System.Attribute
        {
            List<T> attrs = new List<T>();
            System.Reflection.FieldInfo fi = e.GetType().GetField(e.ToString());
            object[] objs = fi.GetCustomAttributes(typeof(T), false);
            foreach (object a in objs)
                if (a is T) attrs.Add((T)a);
            return attrs;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum, AllowMultiple = false)]
    public class AttributeAsset : Attribute
    {
        public string Name;

        public AttributeAsset(string name)
        {
            this.Name = name;
        }
    }
}
