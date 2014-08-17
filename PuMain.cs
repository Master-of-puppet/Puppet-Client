using System;
using System.Collections.Generic;

namespace Puppet
{
    /// <summary>
    /// Author: Dung Nguyen Viet
    /// Puppet Main class
    /// </summary>
    public class PuMain : Patterns.BaseSingleton<PuMain>
    {
        static IPuSettings _settings = null;
        public static IPuSettings Setting 
        {
            get
            {
                if (_settings == null)
                    throw new NullReferenceException("Please initialized setting for Puppet before use!");
                return _settings;
            }
            set { _settings = value; }
        }

        public override void Init()
        {
            Logger.Log("PuppetMain has been initialized");
        }
    }
}
