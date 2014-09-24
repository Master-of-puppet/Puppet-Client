using Puppet.Core.Model;
using Puppet.Core.Network.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet
{
    public delegate void DelegateAPICallback(bool status, string message);
    public delegate void DelegateAPICallbackObject(bool status, string message, object data);
    public delegate void DelegateAPICallbackHttpRequest(bool status, string message, IHttpResponse data);
    public delegate void DelegateAPICallbackDataGame(bool status, string message, List<DataGame> data);
    public delegate void DelegateAPICallbackDataChannel(bool status, string message, List<DataChannel> data);
    public delegate void DelegateAPICallbackDataLobby(bool status, string message, List<DataLobby> data);
}
