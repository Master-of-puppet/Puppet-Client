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

    }
}
