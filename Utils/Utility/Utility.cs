using Sfs2X.Entities.Data;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Puppet.Utils
{
    public class Utility
    {
        public static void StartCoroutine(IEnumerator ienumerator)
        {
            try
            {
                while (!ienumerator.MoveNext())
                    break;
            }
            finally
            {
                IDisposable disposable = ienumerator as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
        }

        public static string SFSObjectToString(SFSObject obj)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (string key in obj.GetKeys())
                dict.Add(key, obj.GetData(key).Data);
            return JsonUtil.Serialize(dict);
        }

    }
}
