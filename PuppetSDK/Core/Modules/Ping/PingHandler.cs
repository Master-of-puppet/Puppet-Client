using Puppet.Core.Model;
using Puppet.Core.Modules.Ping;
using Puppet.Core.Network.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core
{
    public class PingHandler : BaseSingleton<PingHandler>
    {
        IPingManager pingManager;

        protected override void Init()
        {
            if (pingManager == null)
                pingManager = new CSFPing();
        }

        internal IPingManager GetAddOn
        {
            get
            {
                return pingManager;
            }
        }

        public void StartPing()
        {
            pingManager.StartPing();
        }

        public void StopPing()
        {
            pingManager.StopPing();
        }

        public int Value
        {
            get { return pingManager.Value; }
        }

        public bool IsRunning
        {
            get { return pingManager.IsRunning; }
        }
    }
}
