using System;
using System.Collections.Generic;

namespace Puppet.Utils.Threading
{
    internal class UnityThread : BaseSingleton<UnityThread>, IThread
    {
        public void QueueOnMainThread(Action action)
        {
            Loom.QueueOnMainThread(action);
        }

        public void QueueOnMainThread(Action action, float time)
        {
            Loom.QueueOnMainThread(action, time);
        }

        public System.Threading.Thread RunAsync(Action a)
        {
            return Loom.RunAsync(a);
        }

        public override void Init()
        {
        }
    }
}
