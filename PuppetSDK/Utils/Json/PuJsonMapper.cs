using System;
using LitJson;
using System.Reflection;

namespace Puppet.Utils
{
    public class PuJsonMapper : JsonMapper
    {
        //public static object ToObject(string type, string json)
        //{
        //    JsonReader reader = new JsonReader(json);
        //    Logger.Log("Type name: {0}", type);
        //    return ReadValue(GetType(type), reader);
        //}

        public static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null)
                return type;

            var typeWithNamespace = Type.GetType("Puppet." + typeName);
            if (typeWithNamespace != null)
                return typeWithNamespace;

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                typeWithNamespace = a.GetType("Puppet." + typeName);
                if (type != null)
                    return type;
                if (typeWithNamespace != null)
                    return typeWithNamespace;
            }
            Logger.Log("Type is null");
            return null;
        }

    }
}