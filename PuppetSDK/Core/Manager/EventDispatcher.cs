using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Manager
{
    public class EventDispatcher
    {
        public event Action<EScene, EScene> onChangeScene;
        public event Action<EMessage, string> onNoticeMessage;
        public event Action<EUpgrade, string, string> onWarningUpgrade;

        internal void SetChangeScene(EScene fromScene, EScene toScene)
        {
            if (onChangeScene != null)
                onChangeScene(fromScene, toScene);
        }

        internal void SetNoticeMessage(EMessage messageType, string message)
        {
            if (onNoticeMessage != null)
                onNoticeMessage(messageType, message);
        }

        internal void SetWarningUpgrade(EUpgrade type, string message, string url)
        {
            if (onWarningUpgrade != null)
                onWarningUpgrade(type, message, url);
        }
    }
}
