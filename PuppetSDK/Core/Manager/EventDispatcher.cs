using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Manager
{
    public class EventDispatcher
    {
        public event Action<EScene, EScene> onChangeScene;

        internal void SetChangeScene(EScene fromScene, EScene toScene)
        {
            if (onChangeScene != null)
                onChangeScene(fromScene, toScene);
        }
    }
}
