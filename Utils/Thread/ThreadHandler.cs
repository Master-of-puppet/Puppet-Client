using System;
using Puppet.Utils.Threading;

namespace Puppet.Utils
{
    public sealed class ThreadHandler : BaseSingleton<ThreadHandler>
    {
         IThread _thread;

        public override void Init()
        {
            _thread = PuMain.Setting.Threading;
        }

        public static void QueueOnMainThread(Action action)
        {
            Instance._thread.QueueOnMainThread(action);
        }

        public static void QueueOnMainThread(Action action, float time)
        {
            Instance._thread.QueueOnMainThread(action, time);
        }

        public static System.Threading.Thread RunAsync(Action a)
        {
            return Instance._thread.RunAsync(a);
        }
    }
}
