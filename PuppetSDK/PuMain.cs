using Puppet.Core.Manager;
using Puppet.Core.Network.Http;
using Puppet.Core.Network.Socket;
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
                    Logger.Log("you did not initialized the Settings for application. Used the default configuration!");
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
        EventDispatcher _eventDispatcher;
        public EventDispatcher Dispatcher
        {
            get
            {
                if (_eventDispatcher == null)
                    _eventDispatcher = new EventDispatcher();
                return _eventDispatcher;
            }
        }
        #endregion

        protected override void Init()
        {
            Logger.Log("PuppetMain has been initialized");
        }

        /// <summary>
        /// Called to initialize
        /// </summary>
        public void Load()
        {
            new LoadConfig();
        }
    }
}
