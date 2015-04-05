using Puppet.Core.Model;
using Puppet.Core.Network.Socket;
using System;
using System.Collections.Generic;

namespace Puppet.Core.Modules.Ping
{
    public interface IPingManager : ISocketAddOn
    {
        void StartPing();
        void StopPing();
        int Value { get; }
        bool IsRunning { get; }
    }
}
