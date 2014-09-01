using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Network.Socket
{
    public interface ISocket
    {
        event Action<string, ISocketResponse> onResponse;
        void AddListener(Action<string, ISocketResponse> onEventResponse);
        void RemoveListener(Action<string, ISocketResponse> onEventResponse);
        bool IsConnected { get; }
        void Connect();
        void Disconnect();
        void Request(ISocketRequest request);
        void Close();
    }

    public interface ISocketRequest
    {
    }

    public interface ISocketResponse
    {
        IDictionary Params { get; }
        string Type { get; }
    }
}
