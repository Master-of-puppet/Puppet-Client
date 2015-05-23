using Puppet.Core.Flow;
using Puppet.Core.Model;
using Puppet.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Puppet
{
    public class PuSession : BaseSingleton<PuSession>
    {
        private bool initialized = false;

        private PuSessionLogin c_login;
        public static PuSessionLogin Login
        {
            get {  return Instance.c_login; }
            set { Instance.c_login = value; }
        }

        private PuGameOption c_option;
        public static PuGameOption Option
        {
            get { return Instance.c_option; }
            set { Instance.c_option = value; }
        }

        internal void Start(bool loadFromCacheSuccess)
        {
            this.initialized = loadFromCacheSuccess;

            #region GAME SESSION
            if (CacheHandler.Instance.HasKey("SessionLogin"))
                Login = CacheHandler.Instance.GetObject("SessionLogin") as PuSessionLogin;
            else
                Login = new PuSessionLogin();
            Logger.Log("Login Session Info: " + Login.ToJson());
            #endregion

            #region GAME OPTION
            if (CacheHandler.Instance.HasKey("GameOption"))
                Option = CacheHandler.Instance.GetObject("GameOption") as PuGameOption;
            else
                Option = new PuGameOption();
            Logger.Log("Game Option Info: " + Option.ToJson());
            #endregion
        }

        internal void SaveSession()
        {
            CacheHandler.Instance.SetObject("SessionLogin", c_login);
            CacheHandler.Instance.SetObject("GameOption", c_option);
        }

        protected override void Init() { }
    }

    [Serializable()]
    public class PuSessionLogin : DataModel
    {
        public string Time { get; private set; }
        public string Token { get; private set; }

        public PuSessionLogin() : base() { }

        public PuSessionLogin(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }

        public void SaveLoginInfo(string token)
        {
            this.Time = DateTime.Now.ToBinary().ToString();
            this.Token = token;
        }

        public void Clear()
        {
            this.Time = string.Empty;
            this.Token = string.Empty;
        }
    }

    [Serializable()]
    public class PuGameOption : DataModel
    {
        public bool isEnableSoundBG = true;

        public bool isEnableSoundEffect = true;

        public bool isAutoSitdown = true;

        public bool isAutoLockScreen = true;

        public PuGameOption()
            : base()
        {
        }

        public PuGameOption(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }
    }
}
