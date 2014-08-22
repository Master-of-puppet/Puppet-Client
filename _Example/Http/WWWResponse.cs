using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puppet.Core.Network.Http
{
    public class WWWResponse : IHttpResponse
    {
        StateResponse _state = StateResponse.None;
        string _error;
        string _data;
        public WWW www;

        public StateResponse State
        {
            get { return _state; }
            set { _state = value; }
        }

        public string Error
        {
            get { return _error; }
            set { _error = value; }
        }

        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}
