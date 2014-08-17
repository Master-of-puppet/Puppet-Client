using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Puppet.Utils.Threading
{
    internal interface IThread
    {
        void QueueOnMainThread(Action action);
        void QueueOnMainThread(Action action, float time);
        Thread RunAsync(Action a);
    }
}
