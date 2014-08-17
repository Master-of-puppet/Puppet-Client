using System;
using Puppet.Utils.Threading;

namespace Puppet.Utils
{
    public sealed class ThreadHandler : Puppet.Patterns.BaseSingleton<ThreadHandler>
    {
         IThread _thread;

        public override void Init()
        {
            switch (PuMain.Setting.Engine)
            {
                case EEngine.Base:
                    _thread = new CsharpThread();
                    break;
                case EEngine.Unity:
                    _thread = new UnityThread();
                    break;
            }
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
