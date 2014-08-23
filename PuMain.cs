using Puppet.Core.Network.Http;
using System;
using System.Collections.Generic;

namespace Puppet
{
    /// <summary>
    /// Author: Dung Nguyen Viet
    /// Puppet Main class
    /// </summary>
    public class PuMain : BaseSingleton<PuMain>
    {
        static IPuSettings _settings = null;
        public static IPuSettings Setting 
        {
            get
            {
                if (_settings == null)
                {
                    _settings = PuSetting.Instance;
                    Logger.Log("you did not initialized the Settings for application. Used the default configuration!");
                }
                return _settings;
            }
            set { _settings = value; }
        }

        public override void Init()
        {
            Logger.Log("PuppetMain has been initialized");
        }

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
    }
}
