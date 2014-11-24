using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MiniJSON;
using System.Text;

namespace Puppet.Utils
{
    public static class JsonUtil
    {
        public static Dictionary<string, object> Deserialize(string json)
        {
            return Json.Deserialize(json) as Dictionary<string, object>;
        }

        public static string GetStringDecode(string encodeStr)
        {
            return string.IsNullOrEmpty(encodeStr) ? encodeStr : MiniJSON.Json.GetStringDecode(" " + encodeStr);
        }

        public static string Serialize(object obj)
        {
            // Replace the key name from <property>k__BackingField to only the name of property
            string newString = Regex.Replace(Json.Serialize(obj), @"<(\w+)>k__BackingField", "$1");
            newString = Regex.Replace(newString, "(\\\\+)\"", "\"");
            newString = Regex.Replace(newString, "\"\\{", "{");
            newString = Regex.Replace(newString, "\\}\"", "}");

            while (newString.IndexOf("\\\\\\") >= 0)
                newString = newString.Replace("\\\\\\", "\\\\");

            return newString;
        }

        public static bool GetBoolValue(Dictionary<string, object> dict, string key)
        {
            return dict != null ? (bool)dict[key] : false;
        }

        public static string GetStringValue(Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key))
            {
                return GetStringValue(dict[key]);
            }

            return null;
        }

        public static int GetIntValue(Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key))
            {
                return GetIntValue(dict[key]);
            }

            return -1;
        }

        public static string GetStringValue(object obj)
        {
            return obj != null ? (string)obj : null;
        }

        public static int GetIntValue(object obj)
        {
            try
            {
                return obj != null ? (int)obj : 0;
            }
            catch (InvalidCastException)
            {
                try
                {
                    return obj != null ? unchecked((int)((long)obj)) : 0;
                }
                catch (InvalidCastException)
                {
                    return obj != null ? unchecked((int)((double)obj)) : 0;
                }
            }
        }

        public static string DecodeFromUtf8(this string srcString)
        {
            //// read the string as UTF-8 bytes.
            //byte[] encodedBytes = Encoding.UTF8.GetBytes(utf8String);
            //// convert them into unicode bytes.
            //byte[] unicodeBytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, encodedBytes);
            //// builds the converted string.
            //return Encoding.Unicode.GetString(encodedBytes);
            // copy the string as UTF-8 bytes.
            byte[] utf8String = Encoding.UTF8.GetBytes(srcString);
            return Encoding.UTF8.GetString(utf8String);
        }
    }
}

