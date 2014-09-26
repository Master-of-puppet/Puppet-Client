using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Flow;
using Puppet.Core.Network.Http;

namespace Puppet.API.Client
{
    public sealed class APIGeneric
    {
        public static void BackScene(DelegateAPICallback onBackSceneCallback)
        {
            SceneGeneric.Instance.BackScene(onBackSceneCallback);
        }

        public static void ChangeUseInformation(string username, string password, string newpass, DelegateAPICallback callback)
        {
            HttpPool.ChangeUseInformation(username, password, newpass, callback);
        }
    }
}
