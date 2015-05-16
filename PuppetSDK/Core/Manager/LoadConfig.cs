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

            //Step 1: Chạy loading
            Dispatch("Loading");

            //Step 2: Kiểm tra phiên bản
            Dispatch("Checking new version");

            HttpPool.CheckVersion((checkStatus, checkMessage) =>
            {
                //Step 3: Tải thông tin cấu hình
                Dispatch("Loading config");
                HttpPool.GetAppConfig((loadStatus, loadMessage) =>
                {
                    // Gọi để lưu lại danh sách avatar cho lần sau.
                    HttpPool.GetDefaultAvatar(4, null);

                    //Step 4: Load cache từ device
                    Dispatch("Loading from cache");
                    CacheHandler.Instance.LoadFile((status) =>
                    {
                        PuDLCache.Instance.Start();
                        PuSession.Instance.Start(status);

                        //Step 5: Đăng nhập tự động hoặc hoàn tất
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
                Dispatch("Done");
                SceneHandler.Instance.Scene_GoTo(EScene.LoginScreen, string.Empty);
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
