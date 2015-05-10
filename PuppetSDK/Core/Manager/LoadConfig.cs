using System;
using System.Collections.Generic;
using Puppet.Utils;
using Puppet.Core.Model;
using Puppet.Core.Network.Http;
using Puppet.Core.Flow;

namespace Puppet.Core.Manager
{
    internal class LoadConfig
    {
        const int TOTAL_STEP_LOAD = 5;

        Action<float, string> onLoadCallback;

        int countStep = 0;

        internal LoadConfig(Action<float, string> onLoadCallback)
        {
            countStep = 0;
            this.onLoadCallback = onLoadCallback;

            Dispatch("Loading");

            Dispatch("Checking new version");

            HttpPool.CheckVersion((checkStatus, checkMessage) =>
            {
                Dispatch("Loading config");

                HttpPool.GetAppConfig((loadStatus, loadMessage) =>
                {
                    Dispatch("Loading from cache");
                    CacheHandler.Instance.LoadFile((status) =>
                    {
                        PuDLCache.Instance.Start();
                        PuSession.Instance.Start(status);

                        AutoLogin();
                    });
                });
            });
        }

        private void AutoLogin()
        {
            if (!string.IsNullOrEmpty(PuSession.Login.Time))
            {
                Dispatch("Logging to System");
                if (!string.IsNullOrEmpty(PuSession.Login.Token))
                    API.Client.APILogin.Login(PuSession.Login.Token, null);
                else
                    API.Client.APILogin.LoginTrial(null);
            }
            else
            {
                ThreadHandler.QueueOnMainThread(() =>
                {
                    Dispatch("Done");
                    SceneHandler.Instance.Scene_GoTo(EScene.LoginScreen, string.Empty);
                });
            }
        }

        void Dispatch(string text)
        {
            ThreadHandler.QueueOnMainThread(() =>
            {
                countStep++;

                if (onLoadCallback != null)
                    onLoadCallback(countStep / (float)TOTAL_STEP_LOAD, text);
            });
        }
    }
}
