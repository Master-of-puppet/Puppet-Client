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

        public override void Init() 
        {
            socket = new CSmartFox(OnResponse);
        }

        void OnResponse(string eventType, ISocketResponse response)
        {
            SceneHandler.Instance.Current.ProcessEvents(eventType, response);

            if(eventType.Equals(SFSEvent.EXTENSION_RESPONSE))
            {
                SFSObject obj = (SFSObject)response.Params[Fields.PARAMS];
                DataTest test = SFSDataModelFactory.CreateDataModel<DataTest>(obj);
                Logger.Log(test.ToString());
            }
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
