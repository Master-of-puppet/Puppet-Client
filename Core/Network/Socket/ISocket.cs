using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Network.Socket
{
    public interface ISocket
    {
        IServerMode ServerBase { get; }
        void Init();
        void Start();
        void Connect(Action<bool> action);
        bool IsConnected { get; }
        void Request(ISocketRequest request);
        void Response(ISocketResponse response);
    }

    public interface ISocketRequest
    {

    }

    public interface ISocketResponse
    {

    }
}
