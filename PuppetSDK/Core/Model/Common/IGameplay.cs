using Puppet.Core.Network.Socket;
using System;
using System.Collections.Generic;

namespace Puppet.Core.Model
{
    public interface IGameplay
    {
        void EnterGameplay();
        void ExitGameplay();
        void ProcessEvents(string eventType, ISocketResponse onEventResponse);
        void SendEvents(ISocketRequest request);
    }
}
