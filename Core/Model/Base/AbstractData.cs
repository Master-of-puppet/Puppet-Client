using Puppet.Utils;
using Sfs2X.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Puppet.Core.Model
{
    public class AbstractData : IDataModel
    {
        //public const string objectTypeFieldName = "o_type";

        public Dictionary<string, object> ToDictionary()
        {
            var result = new Dictionary<string, object>();
            System.Type T = GetType();
            FieldInfo[] fields = T.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            foreach (FieldInfo f in fields)
                result.Add(f.Name, f.GetValue(this));

            //result.Add(objectTypeFieldName, T.Name);
            return result;
        }

        public override string ToString()
        {
            return JsonUtil.Serialize(this.ToDictionary());
        }

        public SFSObject ToSFSObject()
        {
            SFSObject obj = SFSObject.NewInstance();
            if (this.GetType().GetProperties().Length > 0)
            {
                foreach (PropertyInfo property in this.GetType().GetProperties())
                {
                    string key = property.Name;
                    object value = property.GetValue(this, null);
                    if (value == null)
                        continue;

                    if (property.PropertyType == typeof(int))
                    {
                        obj.PutInt(key, (int)value);
                    }
                    else if (property.PropertyType == typeof(int[]))
                    {
                        obj.PutIntArray(key, (int[])value);
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        obj.PutUtfString(key, (string)value);
                    }
                    else if (property.PropertyType == typeof(string[]))
                    {
                        obj.PutUtfStringArray(key, (string[])value);
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        obj.PutBool(key, (bool)value);
                    }
                    else if (property.PropertyType == typeof(bool[]))
                    {
                        obj.PutBoolArray(key, (bool[])value);
                    }
                    else if (property.PropertyType.IsSubclassOf(typeof(AbstractData)))
                    {
                        obj.PutSFSObject(key, ((AbstractData)value).ToSFSObject());
                    }
                }
            }
            return obj;
        }
    }
}
