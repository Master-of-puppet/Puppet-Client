using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Network.Socket
{
    public interface ISocket<R, E>
    {
        void AddListener(Action<string, E> onResponse);
        bool IsConnected { get; }
        void Connect();
        void Disconnect();
        void Request(R request);
        void Close();
    }
}
