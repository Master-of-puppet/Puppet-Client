using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Puppet.Utils;

namespace Puppet.Core.Model
{
    [Serializable()]
    public class DataModel : IDataModel, ISerializable
    {
        public const string objectTypeFieldName = "o_type";

        public DataModel()
        {
        }
		
        public DataModel(SerializationInfo info, StreamingContext ctxt)
    	{				
			foreach(PropertyInfo propertyInfo in this.GetType().GetProperties()) 
            {
				try
				{
					propertyInfo.SetValue(this, info.GetValue(propertyInfo.Name, propertyInfo.PropertyType), null);
				}
				catch(SerializationException) 
                {
					if (!propertyInfo.Name.Equals("dict"))
						Logger.Log(String.Format("SerializationException class type:{0} name: {1}", this.GetType().Name, propertyInfo.Name));
					continue;
				}
			}
   	 	}
	
    	public void GetObjectData (SerializationInfo info, StreamingContext ctxt)
    	{
			if (info == null)
                throw new System.ArgumentNullException("info");
			
			if (this.GetType().GetProperties().Length > 0) 
            {
				foreach (PropertyInfo property in this.GetType().GetProperties()) 
                {
					// Skip dict to save disk memory
					if (!property.Name.Equals("dict"))
						info.AddValue(property.Name, property.GetValue(this,null));
				}
			}
    	}
		
		public Dictionary<string, object> ToDictionary()
		{
			var result = new Dictionary<string,object>();
	        System.Type T = GetType();
	        FieldInfo[] fields = T.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
	        foreach (FieldInfo f in fields)
				result.Add(f.Name, f.GetValue(this));

            result.Add(objectTypeFieldName, T.Name);
	        return result;
		}
		
        public override string ToString()
        {
            return JsonUtil.Serialize(this.ToDictionary());
        }
    }
}
