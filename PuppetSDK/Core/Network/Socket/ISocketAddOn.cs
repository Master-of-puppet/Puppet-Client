﻿using System;
using System.Collections.Generic;

namespace Puppet.Core.Network.Socket
{
    public interface ISocketAddOn
    {
        bool Initialized { get; }
        void InitSocket(ISocket socket);
        void Begin();
        void ProcessMessage(string type, ISocketResponse response);
        void End();
    }
}
