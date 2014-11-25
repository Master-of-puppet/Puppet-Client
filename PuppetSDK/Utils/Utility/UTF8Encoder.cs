using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Puppet.Utils
{
    /// <summary>
    /// Author: vietdungvn88@gmail.com
    /// Class UTF8Encoder: Để mã hóa và giải mã ký tự UTF-8
    /// </summary>
    public class UTF8Encoder
    {
        public static string EncodeToUtf8(string normalText)
        {
            if (normalText == null)
                return null;
            return Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(normalText));
        }

        public static string DecodeFromUtf8(string utf8String)
        {
            if (utf8String == null)
                return null;

            byte[] utf8Bytes = new byte[utf8String.Length];
            for (int i = 0; i < utf8String.Length; ++i)
            {
                //( 0 <= utf8String[i] && utf8String[i] <= 255, "the char must be in byte's range");
                utf8Bytes[i] = (byte)utf8String[i];
            }
            return Encoding.UTF8.GetString(utf8Bytes, 0, utf8Bytes.Length);
        }

        public static string EncodeNonAsciiCharacters(string value)
        {
            StringBuilder sb = new StringBuilder();
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(value);

            foreach (byte b in bytes)
            {
                sb.Append("\\u");
                sb.Append(String.Format("{0:x4}", (int)b));
            }
            return sb.ToString();
        }

        public static string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m =>
                {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
        }

        void testUTF8Encoder()
        {
            string originalText = "Nguyễn Việt Dũng";
            Logger.Log("the orginal string: " + originalText);
            string encodedText = EncodeNonAsciiCharacters(originalText);
            Logger.Log("unicode the orginal string: " + encodedText);

            // Create and write a string containing the symbol for Pi.
            string srcString = "\u004e\u0000\u0067\u0000\u0075\u0000\u0079\u0000\u00c5\u001e\u006e\u0000\u0020\u0000\u0056\u0000\u0069\u0000\u00c7\u001e\u0074\u0000\u0020\u0000\u0044\u0000\u0069\u0001\u006e\u0000\u0067\u0000";

            // Convert the UTF-16 encoded source string to UTF-8 and ASCII.
            byte[] utf8String = Encoding.UTF8.GetBytes(srcString);
            byte[] asciiString = Encoding.ASCII.GetBytes(srcString);

            // Write the UTF-8 and ASCII encoded byte arrays. 
            Logger.Log("UTF-8  Bytes: " + BitConverter.ToString(utf8String));
            Logger.Log("ASCII  Bytes: " + BitConverter.ToString(asciiString));

            // Convert UTF-8 and ASCII encoded bytes back to UTF-16 encoded  
            // string and write.
            Logger.Log("UTF-8  Text : " + Encoding.UTF8.GetString(utf8String));
            Logger.Log("ASCII  Text : " + Encoding.ASCII.GetString(asciiString));
        }
    }
}
