using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Puppet
{
    public interface IMainMono
    {
        void BeginCoroutine(IEnumerator routine);
        void EndCoroutine(IEnumerator routine);
    }
}
