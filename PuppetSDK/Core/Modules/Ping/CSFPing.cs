using Puppet.Core.Network.Socket;
using Puppet.Utils;
using Sfs2X.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Sfs2X.Entities;
using sfBuddy = Sfs2X.Entities.Buddy;
using Puppet.Core.Model;
using Sfs2X.Util;

namespace Puppet.Core.Modules.Ping
{
    internal class CSFPing : IPingManager, ISocketAddOn
    {
        CSmartFox socket;
        LagMonitor _lagMonitor;

        #region for ISocketAddOn
        public bool Initialized { get { return socket != null; } }

        public void InitSocket(ISocket socket)
        {
            this.socket = (CSmartFox)socket;
            _lagMonitor = new LagMonitor(this.socket.SmartFox, 1000, 10);
        }

        public void Begin()
        {
        }

        public void End()
        {
        }

        public void ProcessMessage(string type, ISocketResponse response)
        {
        }
        #endregion

        #region for IPingManager
        public void StartPing()
        {
            Utility.SimpleCoroutine(_Start());
        }

        IEnumerator _Start()
        {
            while (!Initialized || socket.IsConnected == false)
                yield return null;

            _lagMonitor.Start();
        }

        public void StopPing()
        {
            _lagMonitor.Stop();
        }

        public int Value
        {
            get { return _lagMonitor.OnPingPong(); }
        }

        public bool IsRunning
        {
            get { return Initialized && _lagMonitor.IsRunning; }
        }
        #endregion
    }
}
