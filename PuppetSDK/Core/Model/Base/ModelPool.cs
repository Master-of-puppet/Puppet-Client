using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Puppet
{
    public class PropertyMap
    {
        public PropertyInfo SourceProperty { get; set; }
        public PropertyInfo TargetProperty { get; set; }
    }

    public static class ModelPool
    {
        static IList<PropertyMap> GetMatchingProperties(Type targetType, Type sourceType)
        {
            var sourceProperties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var targetProperties = targetType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var properties = (from s in sourceProperties
                              from t in targetProperties
                              where s.Name == t.Name &&
                                    s.CanRead &&
                                    t.CanWrite && 
                                    s.PropertyType.IsPublic && 
                                    t.PropertyType.IsPublic &&
                                    s.PropertyType == t.PropertyType 
                                    //&&
                                    //(
                                    //  (s.PropertyType.IsValueType &&
                                    //   t.PropertyType.IsValueType
                                    //  ) ||
                                    //  (s.PropertyType == typeof(string) &&
                                    //   t.PropertyType == typeof(string)
                                    //  )
                                    //)
                              select new PropertyMap
                                         {
                                             SourceProperty = s,
                                             TargetProperty = t
                                         }).ToList();
            return properties;
        }

        /// <summary>
        /// Author: Nguyễn Việt Dũng
        /// Email: vietdungvn88@gmail.com
        /// Được tích hợp sẵn trong AbstractData rồi. Hàm này là hàm cha cho object nhé. Khuyến cáo dùng của AbstractData.UpdateData
        /// </summary>
        /// <param name="target">Đối tượng được sao chép</param>
        /// <param name="source">Đối tượng lấy dữ liệu</param>
        /// <param name="onAttributeChanged">Sự kiện khi một thuộc tính thay đổi</param>
        public static void CopyPropertiesFrom(this object target, object source, Action<string> onAttributeChanged = null)
        {
            var targetType = target.GetType();
            var sourceType = source.GetType();
            var propMap = GetMatchingProperties(targetType, sourceType);

            for (var i = 0; i < propMap.Count; i++)
            {
                var prop = propMap[i];
                var sourceValue = prop.SourceProperty.GetValue(source, null);
                prop.TargetProperty.SetValue(target, sourceValue, null);

                if(onAttributeChanged != null)
                    onAttributeChanged(prop.TargetProperty.Name);
            }
        }

        /// <summary>
        /// Author: Nguyễn Việt Dũng
        /// Email: vietdungvn88@gmail.com
        /// </summary>
        /// <param name="obj">Đối tượng sẽ được nhân bản</param>
        /// <returns>Đối tượng đã được nhân bản</returns>
        public static object CloneObject(this object obj)
        {
            if (obj == null)
                return null;
            Type type = obj.GetType();

            if (type.IsValueType || type == typeof(string))
            {
                return obj;
            }
            else if (type.IsArray)
            {
                Type elementType = Type.GetType(
                     type.FullName.Replace("[]", string.Empty));
                var array = obj as Array;
                Array copied = Array.CreateInstance(elementType, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    copied.SetValue(CloneObject(array.GetValue(i)), i);
                }
                return Convert.ChangeType(copied, obj.GetType());
            }
            else if (type.IsClass)
            {
                object toret = Activator.CreateInstance(obj.GetType());
                FieldInfo[] fields =  type.GetFields(BindingFlags.Public |
                            BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    object fieldValue = field.GetValue(obj);
                    if (fieldValue == null)
                        continue;
                    field.SetValue(toret, CloneObject(fieldValue));
                }

                PropertyInfo[] propertices = type.GetProperties(BindingFlags.Public |
                            BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (PropertyInfo property in propertices)
                {
                    object propertyValue = property.GetValue(obj, null);
                    if (propertyValue == null)
                        continue;
                    property.SetValue(toret, CloneObject(propertyValue), null);
                }

                return toret;
            }
            else
                throw new ArgumentException("Unknown type");
        }
    }
}
