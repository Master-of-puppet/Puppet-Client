using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Model;

namespace Puppet.Core.Manager
{
    public class EventDispatcher
    {
        public event Action<EScene, EScene> onChangeScene;
        public event Action<EMessage, string> onNoticeMessage;
        public event Action<EUpgrade, string, string> onWarningUpgrade;
        public event Action<DataDailyGift> onDailyGift;
        public event Action<DataChat> onChatMessage;
        public event Action<UserInfo> onUserInfoUpdate;

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

        internal void SetDailyGift(DataDailyGift data)
        {
            if (onDailyGift != null)
                onDailyGift(data);
        }

        internal void SetChatMessage(DataChat data)
        {
            if (onChatMessage != null)
                onChatMessage(data);
        }

        internal void SetUpdateUserInfo(UserInfo info)
        {
            if (onUserInfoUpdate != null)
                onUserInfoUpdate(info);
        }
    }
}
