using Puppet.Core;
using Puppet.Core.Flow;
using Puppet.Core.Manager;
using Puppet.Core.Model;
using Puppet.Core.Network.Http;
using Puppet.Core.Network.Socket;
using Puppet.Utils;
using System;
using System.Collections.Generic;

namespace Puppet
{
    /// <summary>
    /// Author: Dung Nguyen Viet
    /// Puppet Main class.
    /// Please use 'PuMain.Instance.Load();' as initialization 
    /// </summary>
    public class PuMain : BaseSingleton<PuMain>
    {

        #region PuSetting
        static IPuSettings _settings = null;
        public static IPuSettings Setting 
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new DefaultSetting();
                    _settings.Init();
                    Logger.Log(ELogColor.GREEN, "you did not initialized the Settings for application. Used the default configuration!");
                }
                return _settings;
            }
            set { _settings = value; }
        }
        #endregion

        #region HttpHandler
        static HttpHandler _wwwHandler;
        public static HttpHandler WWWHandler
        {
            get
            {
                if (_wwwHandler == null)
                    _wwwHandler = new HttpHandler(Setting.ServerModeHttp);
                return _wwwHandler;
            }
        }
        #endregion

        #region Socket Handler
        static SocketHandler _socket;
        public static SocketHandler Socket
        {
            get
            {
                if (_socket == null)
                    _socket = SocketHandler.Instance;
                return _socket;
            }
        }
        #endregion

        #region Event
        static EventDispatcher _eventDispatcher;
        public static EventDispatcher Dispatcher
        {
            get
            {
                if (_eventDispatcher == null)
                    _eventDispatcher = new EventDispatcher();
                return _eventDispatcher;
            }
        }
        #endregion

        #region GameLogic
        static IGameplay _handleGameLogic;
        public static IGameplay GameLogic
        {
            get { return _handleGameLogic; }
            set { _handleGameLogic = value; }
        }
        #endregion

        protected override void Init()
        {
            Logger.Log(ELogColor.GREEN, "PuppetMain has been initialized");

            CacheHandler.Instance.LoadFile((status) =>
            {
                PuSession.Instance.Start(status);

                AutoLogin();
            });
        }

        public Sfs2X.Entities.Room SfsRoom
        {
            get { return RoomHandler.Instance.Current; }
        }

        /// <summary>
        /// Called to initialize
        /// </summary>
        public void Load()
        {
            new LoadConfig();
        }

        private void AutoLogin()
        {
            if (!string.IsNullOrEmpty(PuSession.Login.Time))
            {
                if (!string.IsNullOrEmpty(PuSession.Login.Token))
                    API.Client.APILogin.Login(PuSession.Login.Token, null);
                else
                    API.Client.APILogin.LoginTrial(null);
            }
        }
    }
}
