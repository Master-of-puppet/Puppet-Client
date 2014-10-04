using System;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Entities.Data;
using Sfs2X.Entities;
using Puppet.Utils;
using Puppet.Core.Model.Factory;
using Puppet.Core.Model;
using System.Threading;
using Puppet.Core.Flow;

namespace Puppet.Core.Network.Socket
{
    public class SocketHandler : BaseSingleton<SocketHandler>
    {
        ISocket socket;
        protected override void Init() 
        {
            socket = PuMain.Setting.Socket;

            AddListener(OnResponse);
        }

        void OnResponse(string eventType, ISocketResponse response)
        {
            if (response != null && SceneHandler.Instance.Current != null)
                SceneHandler.Instance.Current.ProcessEvents(eventType, response);
        }

        #region Base
        public void AddListener(Action<string, ISocketResponse> onEventResponse)
        {
            socket.onResponse += onEventResponse;
        }

        public void RemoveListener(Action<string, ISocketResponse> onEventResponse)
        {
            socket.onResponse -= onEventResponse;
        }

        public bool IsConnected
        {
            get { return socket.IsConnected; }
        }

        public void Connect()
        {
            socket.Connect();
        }

        public void Reconnect()
        {
            socket.Reconnect();
        }

        public void Disconnect()
        {
            socket.Disconnect();
        }

        public void Request(ISocketRequest request)
        {
            socket.Request(request);
        }

        public void Close()
        {
            socket.Close();
        }
        #endregion
    }
}
