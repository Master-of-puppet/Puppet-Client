using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core.Model;

namespace Puppet.Core.Manager
{
    public class EventDispatcher
    {
        Action<EScene, EScene> _onPreChangeScene;
        public event Action<EScene, EScene> onPreChangeScene
        {
            add { _onPreChangeScene += value; }
            remove { _onPreChangeScene -= value; }
        }

        Action<EScene, EScene> _onChangeScene;
        public event Action<EScene, EScene> onChangeScene
        {
            add  { _onChangeScene += value; }
            remove { _onChangeScene -= value; }
        }

        Action<EMessage, string> _onNoticeMessage;
        public event Action<EMessage, string> onNoticeMessage
        {
            add { _onNoticeMessage += value; }
            remove { _onNoticeMessage -= value; }
        }

        Action<EUpgrade, string, string> _onWarningUpgrade;
        public event Action<EUpgrade, string, string> onWarningUpgrade
        {
            add { _onWarningUpgrade += value; }
            remove { _onWarningUpgrade -= value; }

        }

        Action<DataDailyGift> _onDailyGift;
        public event Action<DataDailyGift> onDailyGift
        {
            add { _onDailyGift += value; }
            remove { _onDailyGift -= value; }
        }

        Action<DataChat> _onChatMessage;
        public event Action<DataChat> onChatMessage
        {
            add { _onChatMessage += value; }
            remove { _onChatMessage -= value; }
        }

        Action<UserInfo> _onUserInfoUpdate;
        public event Action<UserInfo> onUserInfoUpdate
        {
            add  { _onUserInfoUpdate += value; }
            remove { _onUserInfoUpdate -= value; }
        }

        internal void SetPreChangeScene(EScene fromScene, EScene toScene)
        {
            if (_onPreChangeScene != null)
                _onPreChangeScene(fromScene, toScene);
        }

        internal void SetChangeScene(EScene fromScene, EScene toScene)
        {
            if (_onChangeScene != null)
                _onChangeScene(fromScene, toScene);
        }

        internal void SetNoticeMessage(EMessage messageType, string message)
        {
            if (_onNoticeMessage != null)
                _onNoticeMessage(messageType, message);
        }

        internal void SetWarningUpgrade(EUpgrade type, string message, string url)
        {
            if (_onWarningUpgrade != null)
                _onWarningUpgrade(type, message, url);
        }

        internal void SetDailyGift(DataDailyGift data)
        {
            if (_onDailyGift != null)
                _onDailyGift(data);
        }

        internal void SetChatMessage(DataChat data)
        {
            if (_onChatMessage != null)
                _onChatMessage(data);
        }

        internal void SetUpdateUserInfo(UserInfo info)
        {
            if (_onUserInfoUpdate != null)
                _onUserInfoUpdate(info);
        }
    }
}
