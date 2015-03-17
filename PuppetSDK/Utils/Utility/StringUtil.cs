using System;
using System.Text;

namespace Puppet.Utils
{
    public class StringUtil
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string ConvertToBinary(byte [] data)
        {
            return Convert.ToString(data[20], 2).PadLeft(8, '0');
        }
    }
}
