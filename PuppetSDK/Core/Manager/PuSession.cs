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

        internal void Start(bool loadFromCacheSuccess)
        {
            this.initialized = loadFromCacheSuccess;

            if(CacheHandler.Instance.HasKey("SessionLogin"))
                Login = CacheHandler.Instance.GetObject("SessionLogin") as PuSessionLogin;
            else
                Login = new PuSessionLogin();
            Logger.Log("Login Session Info: " + Login.ToJson());
        }

        internal void SaveSession()
        {
            CacheHandler.Instance.SetObject("SessionLogin", c_login);
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
}
