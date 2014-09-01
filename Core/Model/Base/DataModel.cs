using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Puppet.Utils;
using Sfs2X.Entities.Data;

namespace Puppet.Core.Model
{
    [Serializable()]
    public class DataModel : AbstractData, ISerializable
    {
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
    }
}
