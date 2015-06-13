using Puppet.Utils;
using Sfs2X.Entities.Data;
using Sfs2X.Util;
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

        Action<IDataModel> _onDataChanged;
        public event Action<IDataModel> onDataChanged
        {
            add { _onDataChanged += value; }
            remove { _onDataChanged -= value; }
        }

        Action<IDataModel, string> _onAttributeChange;
        public event Action<IDataModel, string> onAttributeChange
        {
            add { _onAttributeChange += value; }
            remove { _onAttributeChange -= value; }
        }

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
            return ToSFSObject().GetDump();
        }

        public string ToJson()
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
                        obj.PutInt(key, (int)value);
                    else if (property.PropertyType == typeof(int[]))
                        obj.PutIntArray(key, (int[])value);
                    else if (property.PropertyType == typeof(string))
                        obj.PutUtfString(key, (string)value);
                    else if (property.PropertyType == typeof(string[]))
                        obj.PutUtfStringArray(key, (string[])value);
                    else if (property.PropertyType == typeof(bool))
                        obj.PutBool(key, (bool)value);
                    else if (property.PropertyType == typeof(bool[]))
                        obj.PutBoolArray(key, (bool[])value);
                    else if (property.PropertyType == typeof(float))
                        obj.PutFloat(key, (float)value);
                    else if (property.PropertyType == typeof(float[]))
                        obj.PutFloatArray(key, (float[])value);
                    else if (property.PropertyType == typeof(double))
                        obj.PutDouble(key, (double)value);
                    else if (property.PropertyType == typeof(double[]))
                        obj.PutDoubleArray(key, (double[])value);
                    else if (property.PropertyType == typeof(short))
                        obj.PutShort(key, (short)value);
                    else if (property.PropertyType == typeof(short[]))
                        obj.PutShortArray(key, (short[])value);
                    else if (property.PropertyType == typeof(long))
                        obj.PutLong(key, (long)value);
                    else if (property.PropertyType == typeof(long[]))
                        obj.PutLongArray(key, (long[])value);
                    else if (property.PropertyType == typeof(byte))
                        obj.PutByte(key, (byte)value);
                    else if (property.PropertyType == (typeof(AbstractData)))
                        obj.PutSFSObject(key, ((AbstractData)value).ToSFSObject());
                    else if (property.PropertyType.IsSubclassOf(typeof(AbstractData)))
                        obj.PutSFSObject(key, ((AbstractData)value).ToSFSObject());
                    else if (property.PropertyType == typeof(byte[]))
                    {
                        ByteArray byteArray = new ByteArray((byte[])value);
                        obj.PutByteArray(key, byteArray);
                    }
                    else if (property.PropertyType.IsArray)
                    {
                        ISFSArray sfsArray = SFSArray.NewInstance();
                        AbstractData[] array = (AbstractData[])value;
                        foreach(AbstractData data in array)
                            sfsArray.AddSFSObject(data.ToSFSObject());
                        obj.PutSFSArray(key, sfsArray);
                    }
                    else
                        Logger.LogError("Not found suitable for the type of '{0}'", property.PropertyType);
                }
            }
            return obj;
        }

        public void UpdateData(IDataModel data, bool nullAble = true, bool autoDispatch = true)
        {
            this.CopyPropertiesFrom(data, nullAble, (propertyName) =>
            {
                if (autoDispatch && _onAttributeChange != null)
                    _onAttributeChange(this, propertyName);
            });

            if (autoDispatch)
                DispatchDataChanged();
        }

        public void DispatchAttribute(string propertyName, bool autoDispatchDataChanged = true)
        {
            if (_onAttributeChange != null)
                _onAttributeChange(this, propertyName);
            if (autoDispatchDataChanged)
                DispatchDataChanged();
        }

        public void DispatchDataChanged()
        {
            if (_onDataChanged != null)
                _onDataChanged(this);
        }
    }
}
