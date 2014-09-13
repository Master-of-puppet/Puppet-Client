using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Flow;

namespace Puppet.API.Client
{
    public sealed class APIGeneric
    {
        public static void BackScene(DelegateAPICallback onBackSceneCallback)
        {
            SceneGeneric.Instance.BackScene(onBackSceneCallback);
        }
    }
}
