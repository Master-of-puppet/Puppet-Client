using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Poker
{
    public static class EventDispatcher
    {
        public static event Action<string, object> onGameEvent;
        
        internal static void SetGameEvent(string command, object data)
        {
            if (onGameEvent != null)
                onGameEvent(command, data);
        }
    }
}
