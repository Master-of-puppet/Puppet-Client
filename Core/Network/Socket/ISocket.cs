using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Network.Socket
{
    public delegate void SocketMessageDelegate(object sender, ISocketResponse e);
    public delegate void SocketOpenDelegate(object sender, EventArgs e);
    public delegate void SocketErrorDelegate(object sender, EventArgs e);
    public delegate void SocketCloseDelegate(object sender, EventArgs e);

    public interface ISocket
    {
        void Init();
        void Start();
        bool IsConnected { get; }
        void Connect(SocketOpenDelegate openDelegate, SocketMessageDelegate messageDelegate);
        void Disconnect();
        void Request(ISocketRequest request);
        void Dispatch();
        void Close();
    }

    public interface ISocketRequest
    {

    }

    public interface ISocketResponse
    {

    }
}
