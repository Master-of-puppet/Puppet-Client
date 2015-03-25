using Puppet.Core.Network.Socket;
using Puppet.Utils;
using Sfs2X.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Puppet.Core.Modules.Buddy
{
    internal class SFBuddy : IBuddy, ISocketAddOn
    {
        CSmartFox socket;

        public bool Initialized { get { return socket != null; } }

        public void Begin(ISocket socket)
        {
            this.socket = (CSmartFox)socket;

            foreach (FieldInfo info in Utility.GetFieldInfo(typeof(SFSBuddyEvent), BindingFlags.Public | BindingFlags.Static))
                this.socket.SmartFox.AddEventListener(info.GetValue(null).ToString(), ListenerDelegate);
        }

        public void End()
        {
            foreach (FieldInfo info in Utility.GetFieldInfo(typeof(SFSBuddyEvent), BindingFlags.Public | BindingFlags.Static))
                this.socket.SmartFox.RemoveEventListener(info.GetValue(null).ToString(), ListenerDelegate);
        }

        public void ProcessMessage(string type, ISocketResponse response)
        {
        }

        void ListenerDelegate(BaseEvent evt)
        {
            if (PuMain.Setting.IsDebug)
            {
                Logger.Log("SFBuddy: type [" + evt.Type + "]", JsonUtil.Serialize(evt.Params), ELogColor.CYAN);
            }

            if (evt.Type == SFSBuddyEvent.BUDDY_ADD)
            {

            }
        }

    }
}
