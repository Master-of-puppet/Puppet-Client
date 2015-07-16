using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Model;

namespace Puppet.Core.Manager
{
    public class ClientDispatcher
    {
        Action<ENetworkDataType, ENetworkDataType> _onNetworkConnectChange;
        public event Action<ENetworkDataType, ENetworkDataType> onNetworkConnectChange
        {
            add { _onNetworkConnectChange += value; }
            remove { _onNetworkConnectChange -= value; }
        }

        public void SetClientChangeNetworkData(ENetworkDataType fromData, ENetworkDataType toData)
        {
            if (_onNetworkConnectChange != null)
                _onNetworkConnectChange(fromData, toData);
        }
    }
}
