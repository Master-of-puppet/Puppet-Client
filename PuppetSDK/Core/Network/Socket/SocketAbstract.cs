using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Network.Socket
{
    public abstract class SocketAbstract : ISocket
    {
        public event Action<string, ISocketResponse> onResponse;
        protected List<ISocketAddOn> listAddOn = new List<ISocketAddOn>();

        public virtual void AddListener(Action<string, ISocketResponse> onEventResponse)
        {
            this.onResponse += onEventResponse;
        }

        public virtual void RemoveListener(Action<string, ISocketResponse> onEventResponse)
        {
            this.onResponse -= onEventResponse;
        }

        public virtual void DispatchResponse(string type, ISocketResponse response)
        {
            if (this.onResponse != null)
                this.onResponse(type, response);

            foreach(ISocketAddOn addOn in listAddOn)
                addOn.ProcessMessage(type, response);
        }

        public virtual void AddPlugin(ISocketAddOn addOn)
        {
            if(!listAddOn.Contains(addOn))
                listAddOn.Add(addOn);
        }

        public virtual void RemovePlugin(ISocketAddOn addOn)
        {
            if (listAddOn.Contains(addOn))
                listAddOn.Remove(addOn);
        }

        public virtual void Connect()
        {
            foreach (ISocketAddOn addOn in listAddOn)
                if(!addOn.Initialized)
                    addOn.Begin(this);
        }

        public virtual void Disconnect()
        {
            foreach (ISocketAddOn addOn in listAddOn)
                if (addOn.Initialized)
                    addOn.End();
        }

        public abstract void Reconnect();
        public abstract bool IsConnected { get; }
        public abstract void Request(ISocketRequest request);
        public abstract void Close();
    }
}
