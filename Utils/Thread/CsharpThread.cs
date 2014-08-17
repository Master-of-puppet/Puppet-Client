using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Puppet.Utils.Threading
{
    class CsharpThread : IThread
    {
        public void QueueOnMainThread(Action action)
        {
            Thread thread = new Thread(new ThreadStart(action));
            thread.Start();
        }

        public void QueueOnMainThread(Action action, float time)
        {
            Thread thread = new Thread(new ThreadStart(() => {
                Thread.Sleep(TimeSpan.FromSeconds(time));
                action();
            }));
            thread.Start();
        }

        public Thread RunAsync(Action action)
        {
            Thread thread = new Thread(new ThreadStart(action));
            thread.Start();
            return thread;
        }

        private static void RunAction(object action)
        {
            ((Action)action)();
        }
    }
}
